using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EatOnCollision : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnEat = new UnityEvent();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Eat(collision.gameObject);
    }

    private void Eat(GameObject food)
    {
        OnEat.Invoke();

        food.GetComponent<Draggable>().OnIsEaten.Invoke();

        Destroy(food.GetComponent<Draggable>().shadow);
        Destroy(food);
    }
}
