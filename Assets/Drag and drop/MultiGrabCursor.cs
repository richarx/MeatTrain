using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Drag_and_drop
{
    public class MultiGrabCursor : MonoBehaviour
    {
        public static MultiGrabCursor instance;

        [SerializeField] private GameObject cursorCollider;

        private Camera mainCamera;
        
        private bool isGrabbing;
        public bool IsGrabbing => isGrabbing;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            UpdateGraphicsState();
        }

        private void Update()
        {
            if (isGrabbing != Mouse.current.leftButton.isPressed)
            {
                isGrabbing = Mouse.current.leftButton.isPressed;
                UpdateGraphicsState();
            }

            if (isGrabbing)
                FollowCursor();
        }

        private void UpdateGraphicsState()
        {
            cursorCollider.SetActive(isGrabbing);
            Cursor.visible = !isGrabbing;
        }

        private void FollowCursor()
        {
            Vector3 cursorPixelPosition = Input.mousePosition;
            Vector3 cursorScreenPosition = new Vector3(cursorPixelPosition.x, cursorPixelPosition.y, 10);

            transform.position = mainCamera.ScreenToWorldPoint(cursorScreenPosition);
        }
    }
}