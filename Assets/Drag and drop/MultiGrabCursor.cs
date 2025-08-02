using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiGrabCursor : MonoBehaviour
{
    public static MultiGrabCursor instance;

    [SerializeField] private GameObject cursorCollider;

    private bool isGrabbing;
    public bool IsGrabbing => isGrabbing;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isGrabbing != Mouse.current.leftButton.isPressed)
        {
            isGrabbing = Mouse.current.leftButton.isPressed;
            UpdateGrapState();
        }

        if (isGrabbing)
            FollowCursor();

    }

    private void UpdateGrapState()
    {
        cursorCollider.SetActive(isGrabbing);
        Cursor.visible = !isGrabbing;
    }

    private void FollowCursor()
    {
        Vector3 cursorPixelPosition = Input.mousePosition;
        Vector3 cursorScreenPosition = new Vector3(cursorPixelPosition.x, cursorPixelPosition.y, 10);

        transform.position = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
    }
}