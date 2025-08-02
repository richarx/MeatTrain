using Drag_and_drop;
using Locomotor;
using UnityEngine;

namespace Entities
{
    public class Scrollable : MonoBehaviour
    {
        private float currentSpeed;

        private Draggable draggable;
        private Digestable digestable;


        void Start()
        {
            draggable = GetComponent<Draggable>();
            digestable = GetComponent<Digestable>();
        }

        void Update()
        {
            if (draggable == null || digestable == null)
            {
                Scroll();
                return;
            }

            if (!draggable.IsBeingDragged && !draggable.IsFalling && !digestable.isBeingDigested)
                Scroll();
        }

        private void Scroll()
        {
            if (GreatLocomotor.instance != null)
            {
                currentSpeed = GreatLocomotor.instance.CurrentSpeed * 7f;
                transform.position += (Vector3.left * (currentSpeed * Time.deltaTime));
            }
        }
    }
}
