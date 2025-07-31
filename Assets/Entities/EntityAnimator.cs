using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    private Draggable draggable;
    private Animator animator;

    void Start()
    {
        draggable = GetComponent<Draggable>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null)
            return;

        if (draggable.IsBeingDragged)
            animator.Play("Grabbed");
        else
            animator.Play("Idle");
    }
}
