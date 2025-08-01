using Locomotor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegsAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GreatLocomotor.instance.CurrentSpeed > 0)
        {
            animator.speed = GreatLocomotor.instance.CurrentSpeed;
            animator.Play("Moving");
        }
        else
            animator.Play("Idle");
    }
}
