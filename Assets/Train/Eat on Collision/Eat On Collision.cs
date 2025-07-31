using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Eat(collision.gameObject);
    }

    private void Eat(GameObject food)
    {
        Destroy(food.GetComponent<Draggable>().shadow);
        Destroy(food);
    }
}
