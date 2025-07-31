using Locomotor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollable : MonoBehaviour
{
    private float currentSpeed;

    private Draggable draggable;

    void Start()
    {
        draggable = GetComponent<Draggable>();
    }

    void Update()
    {
        if (!draggable.IsBeingDragged && !draggable.IsFalling)
            Scroll();
    }

    private void Scroll()
    {
        currentSpeed = GreatLocomotor.instance.CurrentSpeed * 7f;
        transform.position = transform.position + (Vector3.left * currentSpeed * Time.deltaTime);
    }
}
