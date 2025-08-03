using System;
using Parallax;
using System.Collections;
using End_Scene;
using MiniMap;
using Train.Eat_on_Collision;
using UnityEngine;
using UnityEngine.Events;

namespace Locomotor
{
    public class GreatLocomotor : MonoBehaviour
    {
        [Header("Move Speed")]
        [SerializeField] private float startingSpeed;
        [SerializeField] private float speedGainPerLevel;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float targetSpeedIncreaseOnInput;
        [SerializeField] private float targetSpeedDeceleration;
        [SerializeField] private float targetSpeedDecreaseOnHold;
        [SerializeField] private float slowInterval;

        public static UnityEvent<float> OnUpdateSpeed = new UnityEvent<float>();

        public static UnityEvent OnAccelerate = new UnityEvent();
        public static UnityEvent OnBrake = new UnityEvent();

        public static GreatLocomotor instance;

        private float targetSpeed;
        private float currentSpeed;
        private float maxSpeed;
        private float distanceCrawled;
        public float CurrentSpeed => currentSpeed;
        public float TargetSpeed => targetSpeed;
        public float DistanceCrawled => distanceCrawled;

        private bool allGasNoBrakes;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            distanceCrawled = Map.instance.WorldSize / 6.0f;
            LevelHandler.LevelHandler.OnLevelChange.AddListener(UpdateMaxSpeed);
            UpdateMaxSpeed(1);
            
            EndScene.OnTriggerEndScene.AddListener(() => allGasNoBrakes = true);
        }

        private void Update()
        {
            if (MeatWagon.instance.isFull && !allGasNoBrakes)
                DecreaseSpeed();
            
            if (allGasNoBrakes)
                AddSpeed();
            
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
            targetSpeed = Mathf.MoveTowards(targetSpeed, 0.0f, targetSpeedDeceleration * Time.deltaTime);
            OnUpdateSpeed?.Invoke(currentSpeed);

            distanceCrawled += currentSpeed * Time.deltaTime * 7.0f;
        }

        public void AddSpeed()
        {
            if (MeatWagon.instance.isFull && !allGasNoBrakes)
                return;
            
            OnAccelerate.Invoke();
            targetSpeed += targetSpeedIncreaseOnInput;
            targetSpeed = Mathf.Min(targetSpeed, maxSpeed);
        }

        public void DecreaseSpeed()
        {
            if (allGasNoBrakes)
                return;

            OnBrake.Invoke();

            targetSpeed -= targetSpeedDecreaseOnHold * Time.deltaTime;
            targetSpeed = Mathf.Max(targetSpeed, 0);
        }

        private void UpdateMaxSpeed(int level)
        {
            maxSpeed = startingSpeed + speedGainPerLevel * level;
        }
    }
}
