using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MeatWagon : MonoBehaviour
{
    [HideInInspector] public static UnityEvent OnMeatWagonFull = new UnityEvent();
    [HideInInspector] public static UnityEvent<float> OnEat = new UnityEvent<float>();

    [SerializeField] private SpriteRenderer meatSpriteRenderer;
    [SerializeField] public List<Sprite> meatStockSprites;

    private float MeatCount;
    private float MeatMax;
    public float initialMeatMax;

    private void Start()
    {
        MeatMax = initialMeatMax;

        LevelHandler.OnLevelChange.AddListener((_) => UpdateFoodLevel(LevelHandler.Instance.CurrentLevel));
        OnEat.AddListener((_) => UpdateVisualLevel(MeatCount));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Eat(collision.gameObject);
    }

    private void Eat(GameObject food)
    {
        EatSound();
        AddFood(food);
        food.GetComponent<Draggable>().GetEaten();
    }

    private void AddFood(GameObject food)
    {
        if (MeatCount == MeatMax)
            return;

        MeatCount += food.GetComponent<Draggable>().MeatValue;
        Debug.Log(MeatCount);

        OnEat.Invoke(MeatCount);

        if (MeatCount > MeatMax)
            MeatCount = MeatMax;

        if (MeatCount == MeatMax)
            OnMeatWagonFull.Invoke();
    }

    private void UpdateFoodLevel(float level)
    {
        MeatCount = 0;

        if (level == 1)
            MeatMax = 100;
        if (level == 2)
            MeatMax = 250;
        if (level == 3)
            MeatMax = 450;
        if (level == 4)
            MeatMax = 700;
        if (level == 5)
            MeatMax = 1000;
    }

    private void UpdateVisualLevel(float currentMeatLevel)
    {
        float currentMeatPercentage = Tools.Tools.NormalizeValueInRange(MeatCount, 0, MeatMax, 0, 8);

        meatSpriteRenderer.sprite = meatStockSprites[(int)currentMeatPercentage];
    }

    private void EatSound()
    {

    }
}
