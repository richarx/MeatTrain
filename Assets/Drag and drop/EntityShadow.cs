using Entities;
using UnityEngine;

namespace Drag_and_drop
{
    public class EntityShadow : MonoBehaviour
    {
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private LayerMask shadowInteractMask;
    
        private Draggable draggable;
        private Digestable digestable;
    
        private GameObject shadow;
        private RaycastHit2D ray;

        private void Start()
        {
            draggable = GetComponent<Draggable>();
            digestable = GetComponent<Digestable>();
        }

        private void Update()
        {
            if (digestable.isBeingDigested)
            {
                DestroyShadow();
                return;
            }
            
            if (draggable.IsFalling || draggable.IsBeingDragged)
                CreateShadow();
            else
                DestroyShadow();
            
            if (shadow != null)
            {
                if (draggable.IsBeingDragged)
                    ShadowFollow();
                else
                    DetachShadow();
                ShadowScale();
            }
        }

        private void CreateShadow()
        {
            if (shadow == null)
                shadow = Instantiate(shadowPrefab, transform.position - new Vector3(0, draggable.DropHeight, 0.0f), Quaternion.identity);
        }
        
        private void DestroyShadow()
        {
            if (shadow != null)
                Destroy(shadow);
        }

        private void ShadowFollow()
        {
            //4.3 f min ground height size - 0.5 max ground height size
            shadow.transform.parent = transform;
            Vector3 position = transform.position;
            position.y = Mathf.Clamp(position.y - draggable.DropHeight, -6.0f, 0.5f);
            shadow.transform.position = position;
        }
        
        private void DetachShadow()
        {
            shadow.transform.parent = null;
        }

        private void ShadowScale()
        {
            ray = Physics2D.Raycast(transform.position, Vector2.down, 1000f, shadowInteractMask);
            float shadowScale = Mathf.Clamp((10 - ray.distance) / 10, 0.25f, 1f);

            if (shadow != null)
            {
                if (ray.distance > 10f)
                    shadow.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                else if (ray.distance <= 10f && ray.distance > 0f)
                    shadow.transform.localScale = new Vector3(shadowScale, shadowScale, shadowScale);
            }
        }
    }
}
