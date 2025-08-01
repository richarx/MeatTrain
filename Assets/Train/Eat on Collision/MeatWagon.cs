using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class MeatWagon : MonoBehaviour
{
    public static UnityEvent OnMeatWagonFull = new UnityEvent();
    public static UnityEvent<float> OnEat = new UnityEvent<float>();

    [SerializeField] private SpriteRenderer meatSpriteRenderer;
    [SerializeField] public List<Sprite> meatStockSprites;

    [SerializeField] private List<AudioClip> eating;

    private float MeatCount;
    private float MeatMax;

    private SqueezeAndStretch squeeze;
    private SqueezeAndStretch storageSqueeze;

    private void Start()
    {
        squeeze = GetComponent<SqueezeAndStretch>();
        storageSqueeze = meatSpriteRenderer.GetComponent<SqueezeAndStretch>();

        LevelHandler.LevelHandler.OnLevelChange.AddListener(UpdateFoodLevel);
        OnEat.AddListener((_) => UpdateVisualLevel());
        
        UpdateFoodLevel(1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Eat(collision.gameObject);
    }

    private void Eat(GameObject food)
    {
        if (!food.GetComponent<Draggable>().IsFalling)
            return;

        EatSound();
        AddFood(food);
        squeeze.Trigger();
        storageSqueeze.Trigger();
        food.GetComponent<Draggable>().GetEaten();
    }

    private void AddFood(GameObject food)
    {
        if (MeatCount >= MeatMax)
            return;

        MeatCount += food.GetComponent<Draggable>().MeatValue;
        Debug.Log(MeatCount);

        OnEat.Invoke(MeatCount);

        if (MeatCount > MeatMax)
        {
            MeatCount = MeatMax;
            OnMeatWagonFull.Invoke();
        }
    }

    private void UpdateFoodLevel(int level)
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

    private void UpdateVisualLevel()
    {
        float currentMeatPercentage = Tools.Tools.NormalizeValueInRange(MeatCount, 0, MeatMax, 0, 8);
        meatSpriteRenderer.sprite = meatStockSprites[(int)currentMeatPercentage];
    }

    private void EatSound()
    {
        SFXManager.Instance.PlayRandomSFX(eating);
    }
}
