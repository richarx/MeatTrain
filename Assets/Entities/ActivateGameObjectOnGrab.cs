using Drag_and_drop;
using UnityEngine;

namespace Entities
{
    public class ActivateGameObjectOnGrab : MonoBehaviour
    {
        [SerializeField] private GameObject target;

        private void Start()
        {
            Draggable draggable = GetComponent<Draggable>();
            if (draggable != null)
                draggable.OnDrag.AddListener(() => target.SetActive(true));
            target.SetActive(false);
        }
    }
}
