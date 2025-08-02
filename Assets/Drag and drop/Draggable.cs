using Entities;
using Juice;
using UnityEngine;
using UnityEngine.Events;

namespace Drag_and_drop
{
    public class Draggable : MonoBehaviour
    {
        [SerializeField] private float dropSpeed;
        [SerializeField] private float dropHeight;

        [HideInInspector] public UnityEvent OnDrag = new UnityEvent();
        [HideInInspector] public UnityEvent OnDrop = new UnityEvent();
        [HideInInspector] public UnityEvent OnFallGround = new UnityEvent();

        public float DropSpeed => dropSpeed;
        public float DropHeight => dropHeight;
        public bool IsBeingDragged => isBeingDragged;
        private bool isBeingDragged;
        private Vector3 followOffset;
        private float fallHeight;

        public bool IsFalling => isFalling;
        private bool isFalling;

        private SqueezeAndStretch squeeze;
        private Blink blink;
        private Rigidbody2D rb;
        private Digestable digestable;

        private void Start()
        {
            squeeze = GetComponent<SqueezeAndStretch>();
            blink = GetComponent<Blink>();
            rb = GetComponent<Rigidbody2D>();
            digestable = GetComponent<Digestable>();
        }

        private void Update()
        {
            if (digestable.isBeingDigested)
                return;

            if (isBeingDragged)
                FollowCursor();

            if (isBeingDragged && !MultiGrabCursor.instance.IsGrabbing)
                Drop();

            if (isFalling && transform.position.y < fallHeight)
                StopFalling();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {   
            if (MultiGrabCursor.instance.IsGrabbing && collider.CompareTag("Cursor") && !isBeingDragged)
                Drag();
        }

        private void Yeet()
        {

        }

        private void Drag()
        {
            if (digestable.isBeingDigested)
                return;

            OnDrag.Invoke();

            isBeingDragged = true;
            isFalling = false;

            rb.simulated = false;

            float followOffsetx = Random.Range(-0.5f, 0.5f);
            float followOffsety = Random.Range(-0.25f, 0.25f);

            followOffset = new Vector2(followOffsetx, followOffsety);

            if (squeeze != null)
                squeeze.Trigger();

            if (blink != null)
                blink.Trigger();
        }

        private void FollowCursor()
        {
            transform.position = MultiGrabCursor.instance.transform.position + followOffset;
        }

        private void Drop()
        {
            OnDrop.Invoke();

            isBeingDragged = false;
            isFalling = true;

            fallHeight = Mathf.Clamp(transform.position.y - dropHeight + 0.25f, -4.3f, 0.5f);

            rb.simulated = true;
            rb.velocity = Vector2.down * dropSpeed;
        }

        private void StopFalling()
        {
            OnFallGround.Invoke();

            rb.velocity = Vector2.zero;
            isFalling = false;
        }
    }
}
