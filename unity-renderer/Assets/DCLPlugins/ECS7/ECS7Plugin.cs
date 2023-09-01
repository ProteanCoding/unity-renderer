using DCL.Controllers;
using DCL.CRDT;
using DCL.ECS7.ComponentWrapper;
using DCL.ECS7.ComponentWrapper.Generic;
using DCL.ECSComponents;
using DCL.ECSRuntime;
using RPC.Context;
using System.Collections.Generic;

namespace DCL.ECS7
{
    public class ECS7Plugin : IPlugin
    {
        private const int MAX_EXPECTED_SCENES = 81; // Scene Load Radius 4 -> max scenes 81

        private readonly IECSComponentWriter componentWriter;
        private readonly ECS7ComponentsComposer componentsComposer;
        private readonly ECSSystemsController systemsController;
        private readonly ECSComponentsFactory componentsFactory;
        private readonly InternalECSComponents internalEcsComponents;
        private readonly CrdtExecutorsManager crdtExecutorsManager;
        private readonly Dictionary<int, IParcelScene> sceneNumberMapping;
        internal readonly ECSComponentsManager componentsManager;
        private readonly BaseList<IParcelScene> loadedScenes;
        private readonly ISceneController sceneController;
        private readonly SceneStateHandler sceneStateHandler;

        private readonly Dictionary<int, ComponentWriter> componentWriters;
        private readonly Dictionary<int, DualKeyValueSet<long, int, WriteData>> scenesOutgoingMsgs;

        public ECS7Plugin()
        {
            DataStore.i.ecs7.isEcs7Enabled = true;
            loadedScenes = DataStore.i.ecs7.scenes;
            CRDTServiceContext crdtContext = DataStore.i.rpc.context.crdt;
            RestrictedActionsContext rpcRestrictedActionsContext = DataStore.i.rpc.context.restrictedActions;

            sceneController = Environment.i.world.sceneController;
            Dictionary<int, ICRDTExecutor> crdtExecutors = new Dictionary<int, ICRDTExecutor>(MAX_EXPECTED_SCENES);
            crdtContext.CrdtExecutors = crdtExecutors;

            componentWriters = new Dictionary<int, ComponentWriter>(MAX_EXPECTED_SCENES);
            scenesOutgoingMsgs = crdtContext.ScenesOutgoingMsgs;

            componentsFactory = new ECSComponentsFactory();

            componentsManager = new ECSComponentsManager(componentsFactory.componentBuilders);
            internalEcsComponents = new InternalECSComponents(componentsManager, componentsFactory, crdtExecutors);

            crdtExecutorsManager = new CrdtExecutorsManager(crdtExecutors, componentsManager, sceneController, crdtContext);

            componentWriter = new ECSComponentWriter();

            SystemsContext systemsContext = new SystemsContext(
                componentWriters,
                internalEcsComponents,
                new ComponentGroups(componentsManager),
                (ECSComponent<PBBillboard>)componentsManager.GetOrCreateComponent(ComponentID.BILLBOARD),
                (ECSComponent<ECSTransform>)componentsManager.GetOrCreateComponent(ComponentID.TRANSFORM),
                new WrappedComponentPool<IWrappedComponent<PBCameraMode>>(MAX_EXPECTED_SCENES, () => new ProtobufWrappedComponent<PBCameraMode>(new PBCameraMode())),
                new WrappedComponentPool<IWrappedComponent<PBPointerLock>>(MAX_EXPECTED_SCENES, () => new ProtobufWrappedComponent<PBPointerLock>(new PBPointerLock())),
                new WrappedComponentPool<IWrappedComponent<ECSTransform>>(MAX_EXPECTED_SCENES * 2, () => new TransformWrappedComponent(new ECSTransform())),
                new WrappedComponentPool<IWrappedComponent<PBVideoEvent>>(MAX_EXPECTED_SCENES, () => new ProtobufWrappedComponent<PBVideoEvent>(new PBVideoEvent())),
                new WrappedComponentPool<IWrappedComponent<PBRaycastResult>>(MAX_EXPECTED_SCENES, () => new ProtobufWrappedComponent<PBRaycastResult>(new PBRaycastResult())),
                new WrappedComponentPool<IWrappedComponent<PBGltfContainerLoadingState>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBGltfContainerLoadingState>(new PBGltfContainerLoadingState())),
                new WrappedComponentPool<IWrappedComponent<PBEngineInfo>>(MAX_EXPECTED_SCENES, () => new ProtobufWrappedComponent<PBEngineInfo>(new PBEngineInfo())),
                new WrappedComponentPool<IWrappedComponent<PBUiCanvasInformation>>(MAX_EXPECTED_SCENES, () => new ProtobufWrappedComponent<PBUiCanvasInformation>(new PBUiCanvasInformation())),
                new WrappedComponentPool<IWrappedComponent<PBPointerEventsResult>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBPointerEventsResult>(new PBPointerEventsResult())),
                new WrappedComponentPool<IWrappedComponent<PBMeshRenderer>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBMeshRenderer>(new PBMeshRenderer())),
                new WrappedComponentPool<IWrappedComponent<PBMeshCollider>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBMeshCollider>(new PBMeshCollider())),
                new WrappedComponentPool<IWrappedComponent<PBMaterial>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBMaterial>(new PBMaterial())),
                new WrappedComponentPool<IWrappedComponent<PBAnimator>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBAnimator>(new PBAnimator())),
                new WrappedComponentPool<IWrappedComponent<PBAudioSource>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBAudioSource>(new PBAudioSource())),
                new WrappedComponentPool<IWrappedComponent<PBAudioStream>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBAudioStream>(new PBAudioStream())),
                new WrappedComponentPool<IWrappedComponent<PBAvatarShape>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBAvatarShape>(new PBAvatarShape())),
                new WrappedComponentPool<IWrappedComponent<PBBillboard>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBBillboard>(new PBBillboard())),
                new WrappedComponentPool<IWrappedComponent<PBGltfContainer>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBGltfContainer>(new PBGltfContainer())),
                new WrappedComponentPool<IWrappedComponent<PBNftShape>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBNftShape>(new PBNftShape())),
                new WrappedComponentPool<IWrappedComponent<PBPointerEvents>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBPointerEvents>(new PBPointerEvents())),
                new WrappedComponentPool<IWrappedComponent<PBRaycast>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBRaycast>(new PBRaycast())),
                new WrappedComponentPool<IWrappedComponent<PBTextShape>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBTextShape>(new PBTextShape())),
                new WrappedComponentPool<IWrappedComponent<PBUiBackground>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBUiBackground>(new PBUiBackground())),
                new WrappedComponentPool<IWrappedComponent<PBUiDropdown>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBUiDropdown>(new PBUiDropdown())),
                new WrappedComponentPool<IWrappedComponent<PBUiInput>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBUiInput>(new PBUiInput())),
                new WrappedComponentPool<IWrappedComponent<PBUiText>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBUiText>(new PBUiText())),
                new WrappedComponentPool<IWrappedComponent<PBUiTransform>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBUiTransform>(new PBUiTransform())),
                new WrappedComponentPool<IWrappedComponent<PBVideoPlayer>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBVideoPlayer>(new PBVideoPlayer())),
                new WrappedComponentPool<IWrappedComponent<PBVisibilityComponent>>(MAX_EXPECTED_SCENES * 10, () => new ProtobufWrappedComponent<PBVisibilityComponent>(new PBVisibilityComponent()))
            );

            systemsController = new ECSSystemsController(systemsContext);

            componentsComposer = new ECS7ComponentsComposer(componentsFactory, componentWriter, internalEcsComponents);

            sceneNumberMapping = new Dictionary<int, IParcelScene>(MAX_EXPECTED_SCENES); // Scene Load Radius 4 -> max scenes 81

            sceneStateHandler = new SceneStateHandler(
                crdtContext,
                rpcRestrictedActionsContext,
                sceneNumberMapping,
                internalEcsComponents.EngineInfo,
                internalEcsComponents.GltfContainerLoadingStateComponent);

            sceneController.OnNewSceneAdded += SceneControllerOnNewSceneAdded;
            sceneController.OnSceneRemoved += SceneControllerOnSceneRemoved;
        }

        public void Dispose()
        {
            componentsComposer.Dispose();
            componentWriter.Dispose();
            systemsController.Dispose();
            internalEcsComponents.Dispose();
            crdtExecutorsManager.Dispose();
            sceneStateHandler.Dispose();

            sceneController.OnNewSceneAdded -= SceneControllerOnNewSceneAdded;
            sceneController.OnSceneRemoved -= SceneControllerOnSceneRemoved;
        }

        private void SceneControllerOnNewSceneAdded(IParcelScene scene)
        {
            if (!scene.sceneData.sdk7) return;

            int sceneNumber = scene.sceneData.sceneNumber;
            sceneNumberMapping.Add(sceneNumber, scene);
            sceneStateHandler.InitializeEngineInfoComponent(sceneNumber);
            var outgoingMsgs = new DualKeyValueSet<long, int, WriteData>(10);
            scenesOutgoingMsgs.Add(sceneNumber, outgoingMsgs);
            componentWriters.Add(sceneNumber, new ComponentWriter(outgoingMsgs));

            loadedScenes.Add(scene);
        }

        private void SceneControllerOnSceneRemoved(IParcelScene scene)
        {
            if (!scene.sceneData.sdk7) return;

            loadedScenes.Remove(scene);

            int sceneNumber = scene.sceneData.sceneNumber;
            sceneNumberMapping.Remove(sceneNumber);

            if (scenesOutgoingMsgs.TryGetValue(sceneNumber, out var outgoingMsgs))
            {
                var pairs = outgoingMsgs.Pairs;

                for (int i = 0; i < pairs.Count; i++)
                {
                    pairs[i].value.Dispose();
                }
            }

            componentWriters.Remove(sceneNumber);
        }
    }
}
