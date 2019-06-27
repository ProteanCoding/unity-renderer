using DCL.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DCL
{
    public class MouseCatcher : MonoBehaviour, IPointerDownHandler
    {
        //Default OnClick
        public LayerMask OnPointerDownTarget = 1 << 9;
        private Camera characterCamera;

        private void OnEnable()
        {
            RetrieveCharacterCamera();
        }

        private void RetrieveCharacterCamera()
        {
            if (DCLCharacterController.i == null)
            {
                return;
            }

            characterCamera = DCLCharacterController.i.GetComponentInChildren<Camera>();
        }

        void Update()
        {
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnlockCursor();
            }
            #endif

            if (Cursor.lockState != CursorLockMode.None && Input.GetMouseButtonDown(0))
            {
                if (characterCamera == null)
                {
                    RetrieveCharacterCamera();
                }

                //Not sure if this is needed
                if (characterCamera != null)
                {
                    if (Physics.Raycast(
                        characterCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)),
                        out var hitInfo, characterCamera.farClipPlane, OnPointerDownTarget))
                    {
                        hitInfo.rigidbody.gameObject.GetComponentInChildren<OnClickComponent>()?.OnPointerDown();
                    }
                }
            }
        }

        public void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            EventSystem.current.SetSelectedGameObject(null);
        }

        //Externally called by the browser
        public void UnlockCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            LockCursor();
        }
    }
}