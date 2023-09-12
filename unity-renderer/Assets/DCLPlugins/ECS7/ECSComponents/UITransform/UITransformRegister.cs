﻿using DCL.ECS7.ComponentWrapper;
using DCL.ECS7.ComponentWrapper.Generic;
using System;
using DCL.ECS7.InternalComponents;
using DCL.ECSRuntime;

namespace DCL.ECSComponents
{
    public class UITransformRegister : IDisposable
    {
        private readonly ECSComponentsFactory factory;
        private readonly IECSComponentWriter componentWriter;
        private readonly int componentId;

        public UITransformRegister(int componentId, ECSComponentsFactory factory,
            IECSComponentWriter componentWriter, IInternalECSComponent<InternalUiContainer> internalUiContainer)
        {
            var poolWrapper = new ECSReferenceTypeIECSComponentPool<PBUiTransform>(
                new WrappedComponentPool<IWrappedComponent<PBUiTransform>>(10,
                    () => new ProtobufWrappedComponent<PBUiTransform>(new PBUiTransform()))
            );

            var handler = new UITransformHandler(internalUiContainer, componentId);

            factory.AddOrReplaceComponent(
                componentId,
                () => handler,
                ProtoSerialization.Deserialize<PBUiTransform>, // FD::
                iecsComponentPool: poolWrapper // FD:: changed
                );

            componentWriter.AddOrReplaceComponentSerializer<PBUiTransform>(componentId, ProtoSerialization.Serialize);

            this.factory = factory;
            this.componentWriter = componentWriter;
            this.componentId = componentId;
        }

        public void Dispose()
        {
            factory.RemoveComponent(componentId);
            componentWriter.RemoveComponentSerializer(componentId);
        }
    }
}
