using Drag_and_drop;
using Train.Eat_on_Collision;
using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    public class Digestable : MonoBehaviour
    {
        [HideInInspector] public UnityEvent OnIsDigested = new UnityEvent();
        [HideInInspector] public UnityEvent OnStartDigestion = new UnityEvent();

        [SerializeField] private float meatValue;
        [HideInInspector] public float MeatValue => meatValue;

        [SerializeField] private float digestionTime;
        [SerializeField] private float digestionFallCoefficient;
        [SerializeField] private int sortingOrderOffset;

        private float startDigestionTimeStamp = -1f;

        private Rigidbody2D rb;
        private Draggable draggable;

        public bool isBeingDigested => startDigestionTimeStamp > 0;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            draggable = GetComponent<Draggable>();

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

            OnStartDigestion.Invoke();

            startDigestionTimeStamp = Time.time;
            MeatWagon.instance.FoodEntered();

            ChangeSortingOrder();

            rb.velocity = Vector2.down * (draggable.DropSpeed * digestionFallCoefficient);
        }

        private void ChangeSortingOrder()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer spriteRendererTemp = transform.GetChild(i).GetComponent<SpriteRenderer>();

                if (spriteRendererTemp != null)
                    spriteRendererTemp.sortingOrder = -200 + sortingOrderOffset + i;
            }
        }

        private void DestroyAfterDigestion()
        {
            OnIsDigested.Invoke();
            MeatWagon.instance.Eat(this);
            EntitySpawner.instance.DeletePersistentEntity(gameObject);

            Destroy(this.gameObject);
        }
    }
}
