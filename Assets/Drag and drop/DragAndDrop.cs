using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public static DragAndDrop Instance;

    private GameObject objectDragged;

    public bool isDragging => objectDragged != null;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isDragging)
            FollowCursor();
    }

    public void Register(GameObject target)
    {
        if (!isDragging)
            objectDragged = target;
    }

    public void UnRegister()
    {
        if (isDragging)
            objectDragged = null;
    }

    private void FollowCursor()
    {
        Vector3 cursorPixelPosition = Input.mousePosition;
        Vector3 cursorScreenPosition = new Vector3(cursorPixelPosition.x, cursorPixelPosition.y, 10);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);

        objectDragged.transform.position = cursorPosition;
    }
}
