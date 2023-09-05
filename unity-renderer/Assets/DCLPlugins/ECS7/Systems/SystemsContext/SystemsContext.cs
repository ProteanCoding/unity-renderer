using DCL.ECS7;
using DCL.ECS7.ComponentWrapper.Generic;
using DCL.ECSComponents;
using DCL.ECSRuntime;
using System.Collections.Generic;

public record SystemsContext
{
    public readonly IReadOnlyDictionary<int, ComponentWriter> ComponentWriters;
    public readonly IInternalECSComponents InternalEcsComponents;
    public readonly IComponentGroups ComponentGroups;
    public readonly ECSComponent<PBBillboard> Billboards;
    public readonly ECSComponent<ECSTransform> TransformComponent;
    public readonly WrappedComponentPool<IWrappedComponent<PBCameraMode>> CameraModePool;
    public readonly WrappedComponentPool<IWrappedComponent<PBPointerLock>> PointerLockPool;
    public readonly WrappedComponentPool<IWrappedComponent<ECSTransform>> TransformPool;
    public readonly WrappedComponentPool<IWrappedComponent<PBVideoEvent>> VideoEventPool;
    public readonly WrappedComponentPool<IWrappedComponent<PBRaycastResult>> RaycastResultPool;
    public readonly WrappedComponentPool<IWrappedComponent<PBGltfContainerLoadingState>> GltfContainerLoadingStatePool;
    public readonly WrappedComponentPool<IWrappedComponent<PBEngineInfo>> EngineInfoPool;
    public readonly WrappedComponentPool<IWrappedComponent<PBUiCanvasInformation>> UiCanvasInformationPool;
    public readonly WrappedComponentPool<IWrappedComponent<PBPointerEventsResult>> PointerEventsResultPool;



    public SystemsContext(
        IReadOnlyDictionary<int, ComponentWriter> componentWriters,
        IInternalECSComponents internalEcsComponents,
        IComponentGroups componentGroups,
        ECSComponent<PBBillboard> billboards,
        ECSComponent<ECSTransform> transformComponent,
        WrappedComponentPool<IWrappedComponent<PBCameraMode>> cameraModePool,
        WrappedComponentPool<IWrappedComponent<PBPointerLock>> pointerLockPool,
        WrappedComponentPool<IWrappedComponent<ECSTransform>> transformPool,
        WrappedComponentPool<IWrappedComponent<PBVideoEvent>> videoEventPool,
        WrappedComponentPool<IWrappedComponent<PBRaycastResult>> raycastResultPool,
        WrappedComponentPool<IWrappedComponent<PBGltfContainerLoadingState>> gltfContainerLoadingStatePool,
        WrappedComponentPool<IWrappedComponent<PBEngineInfo>> engineInfoPool,
        WrappedComponentPool<IWrappedComponent<PBUiCanvasInformation>> uiCanvasInformationPool,
        WrappedComponentPool<IWrappedComponent<PBPointerEventsResult>> pointerEventsResultPool,
        WrappedComponentPool<IWrappedComponent<PBMeshRenderer>> meshRendererPool,
        WrappedComponentPool<IWrappedComponent<PBMeshCollider>> meshColliderPool,
        WrappedComponentPool<IWrappedComponent<PBMaterial>> materialPool,
        WrappedComponentPool<IWrappedComponent<PBAnimator>> animatorPool,
        WrappedComponentPool<IWrappedComponent<PBAudioSource>> audioSourcePool,
        WrappedComponentPool<IWrappedComponent<PBAudioStream>> audioStreamPool,
        WrappedComponentPool<IWrappedComponent<PBAvatarShape>> avatarShapePool,
        WrappedComponentPool<IWrappedComponent<PBBillboard>> billboardPool,
        WrappedComponentPool<IWrappedComponent<PBGltfContainer>> gltfContainerPool,
        WrappedComponentPool<IWrappedComponent<PBNftShape>> nftShapePool,
        WrappedComponentPool<IWrappedComponent<PBPointerEvents>> pointerEventsPool,
        WrappedComponentPool<IWrappedComponent<PBRaycast>> raycastPool,
        WrappedComponentPool<IWrappedComponent<PBTextShape>> textShapePool,
        WrappedComponentPool<IWrappedComponent<PBUiBackground>> uiBackgroundPool,
        WrappedComponentPool<IWrappedComponent<PBUiDropdown>> uiDropdownPool,
        WrappedComponentPool<IWrappedComponent<PBUiInput>> uiInputPool,
        WrappedComponentPool<IWrappedComponent<PBUiText>> uiTextPool,
        WrappedComponentPool<IWrappedComponent<PBUiTransform>> uiTransformPool,
        WrappedComponentPool<IWrappedComponent<PBVideoPlayer>> videoPlayerPool,
        WrappedComponentPool<IWrappedComponent<PBVisibilityComponent>> visibilityComponentPool)
    {
        this.InternalEcsComponents = internalEcsComponents;
        this.ComponentGroups = componentGroups;
        this.Billboards = billboards;
        this.TransformComponent = transformComponent;
        this.CameraModePool = cameraModePool;
        this.PointerLockPool = pointerLockPool;
        this.TransformPool = transformPool;
        this.ComponentWriters = componentWriters;
        this.VideoEventPool = videoEventPool;
        this.RaycastResultPool = raycastResultPool;
        this.GltfContainerLoadingStatePool = gltfContainerLoadingStatePool;
        this.EngineInfoPool = engineInfoPool;
        this.UiCanvasInformationPool = uiCanvasInformationPool;
        this.PointerEventsResultPool = pointerEventsResultPool;
    }
}
