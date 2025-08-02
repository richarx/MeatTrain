using Drag_and_drop;
using Train.Eat_on_Collision;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class Digestable : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnIsEaten = new UnityEvent();

        [SerializeField] private float meatValue;
        [HideInInspector] public float MeatValue => meatValue;

        [SerializeField] private float digestionTime;
        [SerializeField] private float digestionFallCoefficient;
        [SerializeField] private int sortingOrderOffset;

        private float startDigestionTimeStamp = -1f;

        private Rigidbody2D rb;
        private Draggable draggable;
        private SpriteRenderer spriteRenderer;

        public bool isBeingDigested => startDigestionTimeStamp > 0;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            draggable = GetComponent<Draggable>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            meatValue += Random.Range(-2, 2);
        }

        private void Update()
        {
            if (isBeingDigested && Time.time - startDigestionTimeStamp > digestionTime)
                DestroyAfterDigestion();        
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (!collider.CompareTag("StomachTrain"))
                return;

            if (!isBeingDigested)
                StartDigestion();
        }

        private void StartDigestion()
        {
            if (!draggable.IsFalling)
                return;

            startDigestionTimeStamp = Time.time;
            MeatWagon.instance.FoodEntered();
            draggable.DestroyShadow();

            ChangeSortingOrder();

            rb.velocity = Vector2.down * (draggable.DropSpeed * digestionFallCoefficient);
        }

        private void ChangeSortingOrder()
        {
            spriteRenderer.sortingOrder = -200 + sortingOrderOffset;

            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer spriteRendererTemp = transform.GetChild(i).GetComponent<SpriteRenderer>();

                if (spriteRendererTemp != null)
                    spriteRendererTemp.sortingOrder = -200 + sortingOrderOffset;
            }
        }

        private void DestroyAfterDigestion()
        {
            OnIsEaten.Invoke();
            MeatWagon.instance.Eat(this);

            // Animation etc
            Destroy(this.gameObject);
        }
    }
}
