using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Train.Eat_on_Collision
{
    public class MeatWagon : MonoBehaviour
    {
        public static UnityEvent OnMeatWagonFull = new UnityEvent();
        public static UnityEvent<float> OnEat = new UnityEvent<float>();

        [SerializeField] private SpriteRenderer meatSpriteRenderer;
        [SerializeField] public List<Sprite> meatStockSprites;

        [SerializeField] private List<AudioClip> eating;

        public static MeatWagon instance;
        
        private float MeatCount;
        private float MeatMax;

        public bool isFull => MeatCount >= MeatMax;

        private SqueezeAndStretch squeeze;
        private SqueezeAndStretch storageSqueeze;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            squeeze = GetComponent<SqueezeAndStretch>();
            storageSqueeze = meatSpriteRenderer.GetComponent<SqueezeAndStretch>();

            LevelHandler.LevelHandler.OnLevelChange.AddListener(UpdateFoodLevel);
            OnEat.AddListener((_) => UpdateVisualLevel());
        
            UpdateFoodLevel(1);
        }

        public void FoodEntered()
        {
            EatSound();
            squeeze.Trigger();
        }

        public void Eat(Digestable food)
        {
            AddFood(food);
            storageSqueeze.Trigger(); 
        }

        private void AddFood(Digestable food)
        {
            if (MeatCount >= MeatMax)
                return;

            MeatCount += food.MeatValue;
            Debug.Log(MeatCount);

            OnEat.Invoke(MeatCount);

            if (MeatCount >= MeatMax)
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
            float currentMeatPercentage = Mathf.Clamp(Tools.Tools.NormalizeValueInRange(MeatCount, 0, MeatMax, 0, 8), 0, 8);
            meatSpriteRenderer.sprite = meatStockSprites[(int)currentMeatPercentage];
        }

        public void EatSound()
        {
            SFXManager.Instance.PlayRandomSFX(eating);
        }
    }
}
