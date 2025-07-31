using System;
using Parallax;
using UnityEngine;
using UnityEngine.Events;

namespace Locomotor
{
    public class GreatLocomotor : MonoBehaviour
    {
        [Header("Move Speed")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float acceleration;
        [SerializeField] private float deceleration;
        [SerializeField] private float targetSpeedIncreaseOnInput;
        [SerializeField] private float targetSpeedDeceleration;
        
        public static UnityEvent<float> OnUpdateSpeed = new UnityEvent<float>();

        public static GreatLocomotor instance;

        private float targetSpeed;
        private float currentSpeed;
        private float distanceCrawled;
        public float CurrentSpeed => currentSpeed;
        public float TargetSpeed => targetSpeed;
        public float DistanceCrawled => distanceCrawled;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);
            targetSpeed = Mathf.MoveTowards(targetSpeed, 0.0f, targetSpeedDeceleration * Time.deltaTime);
            OnUpdateSpeed?.Invoke(currentSpeed);

            distanceCrawled += currentSpeed * Time.deltaTime;
        }

        public void AddSpeed()
        {
            targetSpeed += targetSpeedIncreaseOnInput;
            targetSpeed = Mathf.Min(targetSpeed, maxSpeed);
        }
    }
}
