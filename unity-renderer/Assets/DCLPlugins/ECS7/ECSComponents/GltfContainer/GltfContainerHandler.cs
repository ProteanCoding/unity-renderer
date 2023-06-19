using DCL.Components;
using DCL.Controllers;
using DCL.ECS7.InternalComponents;
using DCL.ECSComponents.Utils;
using DCL.ECSRuntime;
using DCL.Helpers;
using DCL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DCL.ECSComponents
{
    public class GltfContainerHandler : IECSComponentHandler<PBGltfContainer>
    {
        private class ActiveCollidersData
        {
            public readonly List<Collider> PhysicColliders = new List<Collider>(10);
            public readonly List<Collider> PointerColliders = new List<Collider>(10);
            public readonly List<Collider> CustomLayerColliders = new List<Collider>(10);
        }

        private const uint LAYER_PHYSICS = (uint)ColliderLayer.ClPhysics;
        private const uint LAYER_POINTER = (uint)ColliderLayer.ClPointer;
        private const string SMR_UPDATE_OFFSCREEN_FEATURE_FLAG = "smr_update_offscreen";

        private readonly IInternalECSComponent<InternalColliders> pointerColliderComponent;
        private readonly IInternalECSComponent<InternalColliders> physicColliderComponent;
        private readonly IInternalECSComponent<InternalColliders> customLayerColliderComponent;
        private readonly IInternalECSComponent<InternalRenderers> renderersComponent;
        private readonly IInternalECSComponent<InternalGltfContainerLoadingState> gltfContainerLoadingStateComponent;

        private readonly DataStore_ECS7 dataStoreEcs7;
        private readonly DataStore_FeatureFlag featureFlags;

        private readonly ActiveCollidersData visibleActiveColliders = new ActiveCollidersData();
        private readonly ActiveCollidersData invisibleActiveColliders = new ActiveCollidersData();

        internal RendereableAssetLoadHelper gltfLoader;
        internal GameObject gameObject;

        private IReadOnlyCollection<Renderer> renderers;

        internal GltfContainerCollidersHandler collidersHandler;
        private PBGltfContainer previousModel = null;

        public GltfContainerHandler(IInternalECSComponent<InternalColliders> pointerColliderComponent,
            IInternalECSComponent<InternalColliders> physicColliderComponent,
            IInternalECSComponent<InternalColliders> customLayerColliderComponent,
            IInternalECSComponent<InternalRenderers> renderersComponent,
            IInternalECSComponent<InternalGltfContainerLoadingState> gltfContainerLoadingStateComponent,
            DataStore_ECS7 dataStoreEcs7,
            DataStore_FeatureFlag featureFlags)
        {
            this.featureFlags = featureFlags;
            this.pointerColliderComponent = pointerColliderComponent;
            this.physicColliderComponent = physicColliderComponent;
            this.customLayerColliderComponent = customLayerColliderComponent;
            this.renderersComponent = renderersComponent;
            this.gltfContainerLoadingStateComponent = gltfContainerLoadingStateComponent;
            this.dataStoreEcs7 = dataStoreEcs7;
        }

        public void OnComponentCreated(IParcelScene scene, IDCLEntity entity)
        {
            gameObject = new GameObject("GLTF mesh");

            Transform transform = gameObject.transform;
            transform.SetParent(entity.gameObject.transform);
            transform.ResetLocalTRS();

            this.contentProvider = scene.contentProvider;
            gltfLoader = new RendereableAssetLoadHelper(scene.contentProvider, scene.sceneData.baseUrlBundles);
            gltfLoader.settings.forceGPUOnlyMesh = true;
            gltfLoader.settings.parent = transform;
            gltfLoader.settings.visibleFlags = AssetPromiseSettings_Rendering.VisibleFlags.VISIBLE_WITH_TRANSITION;
            gltfLoader.settings.smrUpdateWhenOffScreen = DataStore.i.featureFlags.flags.Get().IsFeatureEnabled(SMR_UPDATE_OFFSCREEN_FEATURE_FLAG);

            collidersHandler = new GltfContainerCollidersHandler();

        }

        private ContentProvider contentProvider;
        private LODGroup lodGroup;
        private LOD[] lodsContainer;
        private int lodLoaded;

        private void CheckLOD(string gltfSource)
        {
            if (contentProvider.TryGetContentsUrl_Raw(gltfSource, out string hash))
            {
                string hashLod1 = Application.dataPath + "/../AssetBundles/" + hash + "_lod1";
                if (File.Exists(hashLod1))
                {
                    lodGroup = gameObject.AddComponent<LODGroup>();
                    lodGroup.fadeMode = LODFadeMode.CrossFade;
                    lodGroup.transform.SetParent(gameObject.transform);
                    lodGroup.transform.localPosition = Vector3.zero;
                    lodsContainer = new LOD[3];
                    
                    LOD baseLOD = new LOD();
                    baseLOD.renderers = gameObject.transform.GetComponentsInChildren<MeshRenderer>();
                    baseLOD.screenRelativeTransitionHeight = 0.15f;
                    lodsContainer[0] = baseLOD;

                    LoadLOD($"{hash}_lod1", 1);
                    LoadLOD($"{hash}_lod2", 2);
                }
            }
        }

        private void LoadLOD(string hash, int lodLevel)
        {
            AssetPromise_AB_GameObject abPromise = new AssetPromise_AB_GameObject("", hash);
            abPromise.settings = gltfLoader.settings;

            abPromise.OnSuccessEvent += (x) =>
            {
                LODLoadedSucess(x,hash,lodLevel);
            };

            abPromise.OnFailEvent += (x, exception) => Debug.Log("FAILED TO LOAD LOD " + hash);

            AssetPromiseKeeper_AB_GameObject.i.Keep(abPromise);
        }

        private void LODLoadedSucess(Asset_AB_GameObject lodLoadedAsset, string hash, int lodLevel)
        {
#if UNITY_EDITOR
            lodLoadedAsset.container.name = hash;
#endif
            lodLoadedAsset.container.transform.SetParent(gameObject.transform);


            LOD newLod = new LOD();
            newLod.renderers = lodLoadedAsset.renderers.ToArray();
            newLod.screenRelativeTransitionHeight = lodLevel == 1 ? 0.10f : 0f;
            lodsContainer[lodLevel] = newLod;
            lodLoaded++;

            if (lodLoaded == 2)
            {
                lodGroup.SetLODs(lodsContainer);
                lodGroup.RecalculateBounds();
            }

        }

        public void OnComponentRemoved(IParcelScene scene, IDCLEntity entity)
        {
            UnloadGltf(scene, entity, previousModel?.Src);

            gltfContainerLoadingStateComponent.RemoveFor(scene, entity,
                new InternalGltfContainerLoadingState() { GltfContainerRemoved = true });

            Object.Destroy(gameObject);
        }

        public void OnComponentModelUpdated(IParcelScene scene, IDCLEntity entity, PBGltfContainer model)
        {
            bool srcChanged = previousModel?.Src != model.Src;
            bool visibleMeshColliderMaskChanged = previousModel?.GetVisibleMeshesCollisionMask() != model.GetVisibleMeshesCollisionMask();
            bool invisibleMeshColliderMaskChanged = previousModel?.GetInvisibleMeshesCollisionMask() != model.GetInvisibleMeshesCollisionMask();

            if (srcChanged)
            {
                OnGltfSrcChanged(scene, entity, previousModel, model);
            }
            else
            {
                if (visibleMeshColliderMaskChanged)
                {
                    SetUpColliders(
                        scene,
                        entity,
                        previousModel?.GetVisibleMeshesCollisionMask() ?? 0,
                        model.GetVisibleMeshesCollisionMask(),
                        collidersHandler.GetVisibleMeshesColliders(),
                        pointerColliderComponent,
                        physicColliderComponent,
                        customLayerColliderComponent,
                        visibleActiveColliders.PointerColliders,
                        visibleActiveColliders.PhysicColliders,
                        visibleActiveColliders.CustomLayerColliders);
                }

                if (invisibleMeshColliderMaskChanged)
                {
                    SetUpColliders(
                        scene,
                        entity,
                        previousModel?.GetInvisibleMeshesCollisionMask() ?? 0,
                        model.GetInvisibleMeshesCollisionMask(),
                        collidersHandler.GetInVisibleMeshesColliders(),
                        pointerColliderComponent,
                        physicColliderComponent,
                        customLayerColliderComponent,
                        invisibleActiveColliders.PointerColliders,
                        invisibleActiveColliders.PhysicColliders,
                        invisibleActiveColliders.CustomLayerColliders);
                }
            }

            previousModel = model;
        }

        private void OnGltfSrcChanged(IParcelScene scene, IDCLEntity entity, PBGltfContainer prevModel, PBGltfContainer model)
        {
            if (!string.IsNullOrEmpty(prevModel?.Src))
            {
                UnloadGltf(scene, entity, prevModel.Src);
            }

            string newGltfSrc = model.Src;

            if (!string.IsNullOrEmpty(newGltfSrc))
            {
                gltfContainerLoadingStateComponent.PutFor(scene, entity,
                    new InternalGltfContainerLoadingState() { LoadingState = LoadingState.Loading });

                gltfLoader.OnSuccessEvent += rendereable => OnLoadSuccess(scene,
                    entity, rendereable.renderers, model);

                gltfLoader.OnFailEvent += exception => OnLoadFail(scene, entity, newGltfSrc, exception,
                    dataStoreEcs7, gltfContainerLoadingStateComponent);

                dataStoreEcs7.AddPendingResource(scene.sceneData.sceneNumber, newGltfSrc);
                gltfLoader.Load(newGltfSrc);
            }
        }

        private void OnLoadSuccess(
            IParcelScene scene,
            IDCLEntity entity,
            HashSet<Renderer> rendererHashSet,
            PBGltfContainer model)
        {
            renderers = rendererHashSet;

            InitColliders(
                rendererHashSet,
                gameObject,
                collidersHandler,
                model.GetVisibleMeshesCollisionMask() != 0);

            SetUpRenderers(scene, entity, rendererHashSet, renderersComponent);

            // setup colliders for visible meshes
            SetUpColliders(
                scene,
                entity,
                previousModel?.GetVisibleMeshesCollisionMask() ?? 0,
                model.GetVisibleMeshesCollisionMask(),
                collidersHandler.GetVisibleMeshesColliders(),
                pointerColliderComponent,
                physicColliderComponent,
                customLayerColliderComponent,
                visibleActiveColliders.PointerColliders,
                visibleActiveColliders.PhysicColliders,
                visibleActiveColliders.CustomLayerColliders);

            // setup colliders for invisible meshes
            SetUpColliders(
                scene,
                entity,
                previousModel?.GetInvisibleMeshesCollisionMask() ?? 0,
                model.GetInvisibleMeshesCollisionMask(),
                collidersHandler.GetInVisibleMeshesColliders(),
                pointerColliderComponent,
                physicColliderComponent,
                customLayerColliderComponent,
                invisibleActiveColliders.PointerColliders,
                invisibleActiveColliders.PhysicColliders,
                invisibleActiveColliders.CustomLayerColliders);

            SetGltfLoaded(scene, entity, gameObject, model.Src, gltfContainerLoadingStateComponent, dataStoreEcs7);
            
            CheckLOD(model.Src);
        }

        private void UnloadGltf(IParcelScene scene, IDCLEntity entity, string gltfSrc)
        {
            void RemoveActiveColliders(IList<Collider> colliders, IInternalECSComponent<InternalColliders> colliderComponent)
            {
                for (int i = 0; i < colliders.Count; i++)
                {
                    colliderComponent.RemoveCollider(scene, entity, colliders[i]);
                }

                colliders.Clear();
            }

            RemoveActiveColliders(invisibleActiveColliders.PointerColliders, pointerColliderComponent);
            RemoveActiveColliders(visibleActiveColliders.PointerColliders, pointerColliderComponent);
            RemoveActiveColliders(invisibleActiveColliders.PhysicColliders, physicColliderComponent);
            RemoveActiveColliders(visibleActiveColliders.PhysicColliders, physicColliderComponent);
            RemoveActiveColliders(invisibleActiveColliders.CustomLayerColliders, customLayerColliderComponent);
            RemoveActiveColliders(visibleActiveColliders.CustomLayerColliders, customLayerColliderComponent);

            renderersComponent.RemoveRenderers(scene, entity, renderers);

            if (!string.IsNullOrEmpty(gltfSrc))
            {
                dataStoreEcs7.RemovePendingResource(scene.sceneData.sceneNumber, gltfSrc);
            }

            // TODO: modify Animator component to remove `RemoveShapeReady` usage
            dataStoreEcs7.RemoveShapeReady(entity.entityId);

            collidersHandler.CleanUp();

            gltfLoader.ClearEvents();
            gltfLoader.Unload();
        }

        private static void InitColliders(
            HashSet<Renderer> rendererHashSet,
            GameObject rootGameObject,
            GltfContainerCollidersHandler collidersHandler,
            bool createVisibleMeshColliders)
        {
            MeshFilter[] meshFilters = rootGameObject.GetComponentsInChildren<MeshFilter>();
            collidersHandler.InitInvisibleMeshesColliders(meshFilters);
            collidersHandler.InitVisibleMeshesColliders(rendererHashSet, createVisibleMeshColliders);
        }

        private static void SetUpColliders(
            IParcelScene scene,
            IDCLEntity entity,
            uint prevColliderLayer,
            uint colliderLayer,
            IReadOnlyList<Collider> gltfColliders,
            IInternalECSComponent<InternalColliders> pointerColliderComponent,
            IInternalECSComponent<InternalColliders> physicColliderComponent,
            IInternalECSComponent<InternalColliders> customLayerColliderComponent,
            IList<Collider> currentPointerColliders,
            IList<Collider> currentPhysicColliders,
            IList<Collider> currentCustomLayerColliders)
        {
            if (prevColliderLayer != 0)
            {
                RemoveColliders(scene, entity, prevColliderLayer, pointerColliderComponent,
                    physicColliderComponent, customLayerColliderComponent, currentPointerColliders,
                    currentPhysicColliders, currentCustomLayerColliders);
            }

            if (colliderLayer != 0)
            {
                SetColliders(scene, entity, colliderLayer, gltfColliders, pointerColliderComponent,
                    physicColliderComponent, customLayerColliderComponent, currentPointerColliders,
                    currentPhysicColliders, currentCustomLayerColliders);
            }
        }

        private static void RemoveColliders(
            IParcelScene scene,
            IDCLEntity entity,
            uint colliderLayer,
            IInternalECSComponent<InternalColliders> pointerColliderComponent,
            IInternalECSComponent<InternalColliders> physicColliderComponent,
            IInternalECSComponent<InternalColliders> customLayerColliderComponent,
            IList<Collider> currentPointerColliders,
            IList<Collider> currentPhysicColliders,
            IList<Collider> currentCustomLayerColliders)
        {
            void LocalRemoveColliders(
                IInternalECSComponent<InternalColliders> collidersComponent,
                IList<Collider> currentColliders)
            {
                InternalColliders collidersModel = collidersComponent.GetFor(scene, entity)?.model;

                if (collidersModel != null)
                {
                    for (int i = 0; i < currentColliders.Count; i++)
                    {
                        currentColliders[i].enabled = false;
                        collidersModel.colliders.Remove(currentColliders[i]);
                    }

                    collidersComponent.PutFor(scene, entity, collidersModel);
                }

                currentColliders.Clear();
            }

            if ((colliderLayer & LAYER_PHYSICS) != 0)
            {
                LocalRemoveColliders(physicColliderComponent, currentPhysicColliders);
            }

            if ((colliderLayer & LAYER_POINTER) != 0)
            {
                LocalRemoveColliders(pointerColliderComponent, currentPointerColliders);
            }

            if (LayerMaskUtils.LayerMaskHasAnySDKCustomLayer(colliderLayer))
            {
                LocalRemoveColliders(customLayerColliderComponent, currentCustomLayerColliders);
            }
        }

        private static void SetColliders(
            IParcelScene scene,
            IDCLEntity entity,
            uint colliderLayer,
            IReadOnlyList<Collider> gltfColliders,
            IInternalECSComponent<InternalColliders> pointerColliderComponent,
            IInternalECSComponent<InternalColliders> physicColliderComponent,
            IInternalECSComponent<InternalColliders> customLayerColliderComponent,
            IList<Collider> currentPointerColliders,
            IList<Collider> currentPhysicColliders,
            IList<Collider> currentCustomLayerColliders)
        {
            int? unityGameObjectLayer = LayerMaskUtils.SdkLayerMaskToUnityLayer(colliderLayer);
            bool hasCustomLayer = LayerMaskUtils.LayerMaskHasAnySDKCustomLayer(colliderLayer);

            InternalColliders pointerColliders = pointerColliderComponent.GetFor(scene, entity)?.model;
            InternalColliders physicColliders = physicColliderComponent.GetFor(scene, entity)?.model;
            InternalColliders customColliders = customLayerColliderComponent.GetFor(scene, entity)?.model;

            bool hasPointerColliders = false;
            bool hasPhysicColliders = false;
            bool hasCustomColliders = false;

            for (int i = 0; i < gltfColliders.Count; i++)
            {
                Collider collider = gltfColliders[i];

                if (unityGameObjectLayer.HasValue)
                {
                    collider.gameObject.layer = unityGameObjectLayer.Value;
                }

                collider.enabled = true;

                if ((colliderLayer & LAYER_PHYSICS) != 0)
                {
                    physicColliders ??= new InternalColliders();
                    physicColliders.colliders.Add(collider, colliderLayer);
                    currentPhysicColliders.Add(collider);
                    hasPhysicColliders = true;
                }

                if ((colliderLayer & LAYER_POINTER) != 0)
                {
                    pointerColliders ??= new InternalColliders();
                    pointerColliders.colliders.Add(collider, colliderLayer);
                    currentPointerColliders.Add(collider);
                    hasPointerColliders = true;
                }

                if (hasCustomLayer)
                {
                    customColliders ??= new InternalColliders();
                    customColliders.colliders.Add(collider, colliderLayer);
                    currentCustomLayerColliders.Add(collider);
                    hasCustomColliders = true;
                }
            }

            if (hasPhysicColliders)
            {
                physicColliderComponent.PutFor(scene, entity, physicColliders);
            }

            if (hasPointerColliders)
            {
                pointerColliderComponent.PutFor(scene, entity, pointerColliders);
            }

            if (hasCustomColliders)
            {
                customLayerColliderComponent.PutFor(scene, entity, customColliders);
            }
        }

        private static void SetUpRenderers(
            IParcelScene scene,
            IDCLEntity entity,
            HashSet<Renderer> rendererHashSet,
            IInternalECSComponent<InternalRenderers> renderersComponent)
        {
            if (rendererHashSet == null || rendererHashSet.Count == 0)
                return;

            var model = renderersComponent.GetFor(scene, entity)?.model ?? new InternalRenderers();

            foreach (Renderer renderer in rendererHashSet)
            {
                model.renderers.Add(renderer);
            }

            renderersComponent.PutFor(scene, entity, model);
        }

        private static void SetGltfLoaded(
            IParcelScene scene,
            IDCLEntity entity,
            GameObject rootGameObject,
            string prevLoadedGltf,
            IInternalECSComponent<InternalGltfContainerLoadingState> gltfContainerLoadingStateComponent,
            DataStore_ECS7 dataStoreEcs7)
        {
            gltfContainerLoadingStateComponent.PutFor(scene, entity,
                new InternalGltfContainerLoadingState() { LoadingState = LoadingState.Finished });

            // TODO: modify Animator component to remove `AddShapeReady` usage
            dataStoreEcs7.AddShapeReady(entity.entityId, rootGameObject);

            if (!string.IsNullOrEmpty(prevLoadedGltf))
            {
                dataStoreEcs7.RemovePendingResource(scene.sceneData.sceneNumber, prevLoadedGltf);
            }
        }

        private static void OnLoadFail(
            IParcelScene scene,
            IDCLEntity entity,
            string gltfSrc,
            Exception exception,
            DataStore_ECS7 dataStoreEcs7,
            IInternalECSComponent<InternalGltfContainerLoadingState> gltfContainerLoadingStateComponent)
        {
            gltfContainerLoadingStateComponent.PutFor(scene, entity,
                new InternalGltfContainerLoadingState() { LoadingState = LoadingState.FinishedWithError });

            dataStoreEcs7.RemovePendingResource(scene.sceneData.sceneNumber, gltfSrc);
        }
    }
}
