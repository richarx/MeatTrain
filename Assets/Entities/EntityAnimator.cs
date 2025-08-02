using Drag_and_drop;
using UnityEngine;

namespace Entities
{
    public class EntityAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private Draggable draggable;

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
}
