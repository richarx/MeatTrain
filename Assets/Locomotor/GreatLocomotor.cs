using System;
using Parallax;
using System.Collections;
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

        public static UnityEvent OnHonk = new UnityEvent();
        public static UnityEvent OnAccelerate = new UnityEvent();

        public static GreatLocomotor instance;

        private float targetSpeed;
        private float currentSpeed;
        private float maxSpeed;
        private float distanceCrawled;
        public float CurrentSpeed => currentSpeed;
        public float TargetSpeed => targetSpeed;
        public float DistanceCrawled => distanceCrawled;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            LevelHandler.LevelHandler.OnLevelChange.AddListener(UpdateMaxSpeed);
            UpdateMaxSpeed(1);
        }

        private void Update()
        {
            if (MeatWagon.instance.isFull)
                DecreaseSpeed();
            
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
            targetSpeed = Mathf.MoveTowards(targetSpeed, 0.0f, targetSpeedDeceleration * Time.deltaTime);
            OnUpdateSpeed?.Invoke(currentSpeed);

            distanceCrawled += currentSpeed * Time.deltaTime;
        }

        public void AddSpeed()
        {
            if (MeatWagon.instance.isFull)
                return;
            
            OnAccelerate.Invoke();
            targetSpeed += targetSpeedIncreaseOnInput;
            targetSpeed = Mathf.Min(targetSpeed, maxSpeed);
        }

        public void DecreaseSpeed()
        {
            targetSpeed -= targetSpeedDecreaseOnHold * Time.deltaTime;
            targetSpeed = Mathf.Max(targetSpeed, 0);
        }

        public void Honk()
        {
            OnHonk.Invoke();
            Debug.Log("Honk honk");
        }

        private void UpdateMaxSpeed(int level)
        {
            maxSpeed = startingSpeed + speedGainPerLevel * level;
        }
    }
}
