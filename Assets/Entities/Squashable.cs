using Drag_and_drop;
using Locomotor;
using System.Collections;
using System.Collections.Generic;
using Train.Eat_on_Collision;
using UnityEngine;
using UnityEngine.Events;

public class Squashable : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnSquash = new UnityEvent();

    private Draggable draggable;

    private void Start()
    {
        draggable = GetComponent<Draggable>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("SquashTrain") && GreatLocomotor.instance.CurrentSpeed > 0 && !draggable.IsBeingDragged)
            Squash();
    }

    private void Squash()
    {
        OnSquash.Invoke();
        MeatWagon.instance.EatSound();
        Destroy(this.gameObject);
    }
}
