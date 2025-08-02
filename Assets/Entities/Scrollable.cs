using Locomotor;
using Entities;
using UnityEngine;

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
