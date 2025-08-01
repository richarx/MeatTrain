using System;
using Parallax;
using System.Collections;
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

        public float SlowInterval => slowInterval;

        [HideInInspector] public static UnityEvent<float> OnUpdateSpeed = new UnityEvent<float>();

        [HideInInspector] public static UnityEvent OnHonk = new UnityEvent();
        [HideInInspector] public static UnityEvent OnAccelerate = new UnityEvent();
        [HideInInspector] public static UnityEvent OnDecelerate = new UnityEvent();

        public static GreatLocomotor instance;

        private float targetSpeed;
        private float currentSpeed;
        private float maxSpeed;
        private float distanceCrawled;
        public float CurrentSpeed => currentSpeed;
        public float TargetSpeed => targetSpeed;
        public float DistanceCrawled => distanceCrawled;

        public bool isBreaking;
        private bool hasBreaked;

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
            if (isBreaking == true && hasBreaked == false)
            {
                StartCoroutine(DecreaseSpeed());
                hasBreaked = true;
            }

            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
            targetSpeed = Mathf.MoveTowards(targetSpeed, 0.0f, targetSpeedDeceleration * Time.deltaTime);
            OnUpdateSpeed?.Invoke(currentSpeed);

            distanceCrawled += currentSpeed * Time.deltaTime;
        }

        public void AddSpeed()
        {
            OnAccelerate.Invoke();
            targetSpeed += targetSpeedIncreaseOnInput;
            targetSpeed = Mathf.Min(targetSpeed, maxSpeed);
        }

        public IEnumerator DecreaseSpeed()
        {
            OnDecelerate.Invoke();

            while (isBreaking)
            {
                targetSpeed -= targetSpeedDecreaseOnHold;
                targetSpeed = Mathf.Max(targetSpeed, 0);
                yield return new WaitForSeconds(slowInterval);
            }

            hasBreaked = false;
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
