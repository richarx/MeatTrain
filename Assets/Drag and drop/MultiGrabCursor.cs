using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Drag_and_drop
{
    public class MultiGrabCursor : MonoBehaviour
    {
        public static MultiGrabCursor instance;

        [SerializeField] private TextMeshPro grabCounterTMP;

        [SerializeField] private GameObject cursorCollider;

        private Camera mainCamera;
        private SqueezeAndStretch squeeze;

        private bool isGrabbing;
        public bool IsGrabbing => isGrabbing;

        private int foodGrabbedCount;
        public int FoodGrabbedCount => foodGrabbedCount;
        public bool grabCountPositive => foodGrabbedCount > 0;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;
            UpdateGraphicsState();

            squeeze = GetComponent<SqueezeAndStretch>();
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

        public void AddFoodCount()
        {
            foodGrabbedCount += 1;
            UpdateCount(foodGrabbedCount);
            squeeze.Trigger();
        }

        public void RemoveFoodCount()
        {
            foodGrabbedCount -= 1;
            grabCounterTMP.text = "";
        }

        private void UpdateCount(int foodCount)
        {
            grabCounterTMP.text = $"{foodCount}";
        }
    }
}