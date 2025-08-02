using Drag_and_drop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontMouthAnimator : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;

    private bool hasDragged;

    void Update()
    {
        if (MultiGrabCursor.instance.grabCountPositive != hasDragged)
        {
            hasDragged = MultiGrabCursor.instance.grabCountPositive;

            StopAllCoroutines();

            if (hasDragged)
                StartCoroutine(PlayAnimation("Moving", false));
            else
                StartCoroutine(PlayAnimation("Idle", true));
        }
    }

    private IEnumerator PlayAnimation(string animation, bool wait)
    {
        if (wait)
            yield return new WaitForSeconds(0.5f);

        foreach (Animator animator in animators)
        {
            animator.Play(animation);
        }
    }
}
