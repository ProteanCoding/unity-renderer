using System.Collections;
using System.Collections.Generic;
using DCL;
using DCL.Components;
using DCL.Helpers;
using DCL.Models;
using DCL.Interface;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class UIImageTests : TestsBase
    {
        [UnityTest]
        public IEnumerator TestPropertiesAreAppliedCorrectly()
        {
            yield return InitScene();

            DCLCharacterController.i.gravity = 0f;

            // Position character inside parcel (0,0)
            DCLCharacterController.i.SetPosition(JsonConvert.SerializeObject(new
            {
                x = 0f,
                y = 0f,
                z = 0f
            }));

            // Create UIScreenSpaceShape
            UIScreenSpace screenSpaceShape = TestHelpers.SharedComponentCreate<UIScreenSpace, UIScreenSpace.Model>(scene, CLASS_ID.UI_SCREEN_SPACE_SHAPE);
            yield return screenSpaceShape.routine;

            // Create UIImage
            UIImage uiImageShape = TestHelpers.SharedComponentCreate<UIImage, UIImage.Model>(scene, CLASS_ID.UI_IMAGE_SHAPE);
            yield return uiImageShape.routine;

            // Check default properties are applied correctly
            Assert.IsTrue(uiImageShape.referencesContainer.transform.parent == screenSpaceShape.childHookRectTransform);
            Assert.IsTrue(uiImageShape.referencesContainer.image.color == new Color(1, 1, 1, 1));
            Assert.IsTrue(uiImageShape.referencesContainer.canvasGroup.blocksRaycasts);
            Assert.AreEqual(100f, uiImageShape.childHookRectTransform.rect.width);
            Assert.AreEqual(100f, uiImageShape.childHookRectTransform.rect.height);
            Assert.IsTrue(uiImageShape.referencesContainer.image.enabled);
            Assert.IsTrue(uiImageShape.referencesContainer.image.texture == null);

            Vector2 alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect);
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());

            // Update UIImage properties
            DCLTexture texture = TestHelpers.CreateDCLTexture(scene, TestHelpers.GetTestsAssetsPath() + "/Images/atlas.png");
            yield return texture.routine;

            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                parentComponent = screenSpaceShape.id,
                source = texture.id,
                opacity = 0.7f,
                isPointerBlocker = true,
                width = new UIValue(128f),
                height = new UIValue(128f),
                positionX = new UIValue(-55f),
                positionY = new UIValue(30f),
                hAlign = "right",
                vAlign = "bottom",
                sourceLeft = 64f,
                sourceTop = 64f,
                sourceWidth = 64f,
                sourceHeight = 64f,
                paddingTop = 10f,
                paddingRight = 10f,
                paddingBottom = 10f,
                paddingLeft = 10f
            });
            yield return uiImageShape.routine;

            // Check default properties are applied correctly
            Assert.IsTrue(uiImageShape.referencesContainer.transform.parent == screenSpaceShape.childHookRectTransform);
            Assert.AreEqual(new Color(1, 1, 1, 1), uiImageShape.referencesContainer.image.color);
            Assert.IsTrue(uiImageShape.referencesContainer.canvasGroup.blocksRaycasts);

            Assert.AreEqual(108f, uiImageShape.childHookRectTransform.rect.width); // 128 - 20 (left and right padding)
            Assert.AreEqual(108f, uiImageShape.childHookRectTransform.rect.height); // 128 - 20 (left and right padding)
            Assert.IsTrue(uiImageShape.referencesContainer.image.enabled);
            Assert.IsTrue(uiImageShape.referencesContainer.image.texture != null);

            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "bottom", "right");
            alignedPosition += new Vector2(-65f, 40f); // affected by padding

            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());

            Assert.AreEqual(0.5f, uiImageShape.referencesContainer.image.uvRect.x);
            Assert.AreEqual(0f, uiImageShape.referencesContainer.image.uvRect.y);
            Assert.AreEqual(0.5f, uiImageShape.referencesContainer.image.uvRect.width);
            Assert.AreEqual(0.5f, uiImageShape.referencesContainer.image.uvRect.height);

            Assert.AreEqual(10, uiImageShape.referencesContainer.paddingLayoutGroup.padding.bottom);
            Assert.AreEqual(10, uiImageShape.referencesContainer.paddingLayoutGroup.padding.top);
            Assert.AreEqual(10, uiImageShape.referencesContainer.paddingLayoutGroup.padding.left);
            Assert.AreEqual(10, uiImageShape.referencesContainer.paddingLayoutGroup.padding.right);

            screenSpaceShape.Dispose();
        }

        [UnityTest]
        public IEnumerator TestMissingValuesGetDefaultedOnUpdate()
        {
            yield return InitScene();

            UIScreenSpace screenSpaceShape = TestHelpers.SharedComponentCreate<UIScreenSpace, UIScreenSpace.Model>(scene, CLASS_ID.UI_SCREEN_SPACE_SHAPE);
            yield return screenSpaceShape.routine;

            Assert.IsFalse(screenSpaceShape == null);

            yield return TestHelpers.TestSharedComponentDefaultsOnUpdate<UIImage.Model, UIImage>(scene, CLASS_ID.UI_IMAGE_SHAPE);
        }

        [UnityTest]
        public IEnumerator TestOnclickEvent()
        {
            yield return InitScene();

            UIScreenSpace screenSpaceShape = TestHelpers.SharedComponentCreate<UIScreenSpace, UIScreenSpace.Model>(scene, CLASS_ID.UI_SCREEN_SPACE_SHAPE);
            yield return screenSpaceShape.routine;

            Assert.IsFalse(screenSpaceShape == null);

            DCLTexture texture = TestHelpers.CreateDCLTexture(scene, TestHelpers.GetTestsAssetsPath() + "/Images/atlas.png");
            yield return texture.routine;

            UIImage uiImage = TestHelpers.SharedComponentCreate<UIImage, UIImage.Model>(scene, CLASS_ID.UI_IMAGE_SHAPE);
            yield return uiImage.routine;

            string uiImageOnClickEventId = "UUIDFakeEventId";

            TestHelpers.SharedComponentUpdate(scene, uiImage, new UIImage.Model
            {
                parentComponent = screenSpaceShape.id,
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                onClick = uiImageOnClickEventId
            });
            yield return uiImage.routine;

            // We need to populate the event data with the 'pointerPressRaycast' pointing to the 'clicked' object
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            RaycastResult raycastResult = new RaycastResult();
            raycastResult.gameObject = uiImage.referencesContainer.image.gameObject;
            pointerEventData.pointerPressRaycast = raycastResult;

            string targetEventType = "SceneEvent";

            var onClickEvent = new WebInterface.OnClickEvent();
            onClickEvent.uuid = uiImageOnClickEventId;

            var sceneEvent = new WebInterface.SceneEvent<WebInterface.OnClickEvent>();
            sceneEvent.sceneId = scene.sceneData.id;
            sceneEvent.payload = onClickEvent;
            sceneEvent.eventType = "uuidEvent";
            string eventJSON = JsonUtility.ToJson(sceneEvent);

            bool eventTriggered = false;

            yield return TestHelpers.WaitForMessageFromEngine(targetEventType, eventJSON,
            () =>
            {
                ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
            },
            () =>
            {
                eventTriggered = true;
            });

            yield return null;

            // Check image object clicking triggers the correct event
            Assert.IsTrue(eventTriggered);

            // Check UI children won't trigger the parent/root image component event
            UIContainerRect uiContainer = TestHelpers.SharedComponentCreate<UIContainerRect, UIContainerRect.Model>(scene, CLASS_ID.UI_CONTAINER_RECT);
            yield return uiContainer.routine;

            TestHelpers.SharedComponentUpdate(scene, uiContainer, new UIContainerRect.Model
            {
                parentComponent = uiImage.id
            });
            yield return uiContainer.routine;

            raycastResult.gameObject = uiContainer.referencesContainer.image.gameObject;
            pointerEventData.pointerPressRaycast = raycastResult;

            eventTriggered = false;

            yield return TestHelpers.WaitForMessageFromEngine(targetEventType, eventJSON,
            () =>
            {
                ExecuteEvents.ExecuteHierarchy(raycastResult.gameObject, pointerEventData, ExecuteEvents.pointerDownHandler);
            },
            () =>
            {
                eventTriggered = true;
            });

            Assert.IsFalse(eventTriggered);
        }

        [UnityTest]
        public IEnumerator TestOnEnterEvent()
        {
            yield return InitScene();

            UIScreenSpace screenSpaceShape = TestHelpers.SharedComponentCreate<UIScreenSpace, UIScreenSpace.Model>(scene, CLASS_ID.UI_SCREEN_SPACE_SHAPE);
            yield return screenSpaceShape.routine;

            Assert.IsFalse(screenSpaceShape == null);

            DCLTexture texture = TestHelpers.CreateDCLTexture(scene, TestHelpers.GetTestsAssetsPath() + "/Images/atlas.png");
            yield return texture.routine;

            UIImage uiImage = TestHelpers.SharedComponentCreate<UIImage, UIImage.Model>(scene, CLASS_ID.UI_IMAGE_SHAPE);
            yield return uiImage.routine;

            string uiImageOnEnterUUID = "UUIDFakeEventId";

            TestHelpers.SharedComponentUpdate(scene, uiImage, new UIImage.Model
            {
                parentComponent = screenSpaceShape.id,
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                onEnter = uiImageOnEnterUUID
            });
            yield return uiImage.routine;

            var sceneEvent = new WebInterface.SceneEvent<WebInterface.OnEnterEvent>
            {
                sceneId = scene.sceneData.id,
                payload = new WebInterface.OnEnterEvent {uuid = uiImageOnEnterUUID},
                eventType = "uuidEvent"
            };

            bool eventTriggered = false;
            yield return TestHelpers.WaitForMessageFromEngine("SceneEvent", JsonUtility.ToJson(sceneEvent),
                () =>
                {
                    uiImage.referencesContainer.OnEnterPressed();
                },
                () =>
                {
                    eventTriggered = true;
                });

            yield return null;

            Assert.IsTrue(eventTriggered);
        }

        [UnityTest]
        public IEnumerator TestAlignment()
        {
            yield return InitScene();

            DCLCharacterController.i.gravity = 0f;

            // Position character inside parcel (0,0)
            DCLCharacterController.i.SetPosition(JsonConvert.SerializeObject(new
            {
                x = 0f,
                y = 0f,
                z = 0f
            }));

            // Create UIScreenSpaceShape
            UIScreenSpace screenSpaceShape = TestHelpers.SharedComponentCreate<UIScreenSpace, UIScreenSpace.Model>(scene, CLASS_ID.UI_SCREEN_SPACE_SHAPE);
            yield return screenSpaceShape.routine;

            // Create UIImage
            UIImage uiImageShape = TestHelpers.SharedComponentCreate<UIImage, UIImage.Model>(scene, CLASS_ID.UI_IMAGE_SHAPE);
            yield return uiImageShape.routine;

            DCLTexture texture = TestHelpers.CreateDCLTexture(scene, TestHelpers.GetTestsAssetsPath() + "/Images/atlas.png");
            yield return texture.routine;

            // Align to right-bottom
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                parentComponent = screenSpaceShape.id,
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "right",
                vAlign = "bottom"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            Vector2 alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "bottom", "right");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.LowerRight, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to right-center
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "right",
                vAlign = "center"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "center", "right");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.MiddleRight, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to right-top
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "right",
                vAlign = "top"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "top", "right");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.UpperRight, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to center-bottom
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "center",
                vAlign = "bottom"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "bottom", "center");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.LowerCenter, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to center-center
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "center",
                vAlign = "center"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "center", "center");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.MiddleCenter, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to center-top
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "center",
                vAlign = "top"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "top", "center");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.UpperCenter, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to left-bottom
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "left",
                vAlign = "bottom"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "bottom", "left");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.LowerLeft, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to left-center
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "left",
                vAlign = "center"
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "center", "left");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.MiddleLeft, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            // Align to left-top
            TestHelpers.SharedComponentUpdate(scene, uiImageShape, new UIImage.Model
            {
                source = texture.id,
                width = new UIValue(128f),
                height = new UIValue(128f),
                hAlign = "left",
                vAlign = "top",
            });
            yield return uiImageShape.routine;

            // Check alignment position was applied correctly
            alignedPosition = CalculateAlignedPosition(screenSpaceShape.childHookRectTransform.rect, uiImageShape.childHookRectTransform.rect, "top", "left");
            Assert.AreEqual(alignedPosition.ToString(), uiImageShape.referencesContainer.layoutElementRT.anchoredPosition.ToString());
            Assert.AreEqual(TextAnchor.UpperLeft, uiImageShape.referencesContainer.layoutGroup.childAlignment);

            screenSpaceShape.Dispose();
        }

    }
}
