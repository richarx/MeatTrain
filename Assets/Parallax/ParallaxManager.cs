using System;
using System.Collections.Generic;
using Locomotor;
using UnityEngine;

namespace Parallax
{
    public class ParallaxManager : MonoBehaviour
    {
        [SerializeField] private float defaultSpeed;
        [SerializeField] private List<Parallax> parallaxes;

        
        public static ParallaxManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            GreatLocomotor.OnUpdateSpeed.AddListener(SetSpeed);
            
            foreach (Parallax parallax in parallaxes)
            {
                parallax.SetSpeed(defaultSpeed);
            }
        }

        private void Update()
        {
            
        }

        public void StartMoving()
        {
            foreach (Parallax parallax in parallaxes)
            {
                parallax.StartMoving();
            }
        }
        
        public void StopMoving()
        {
            foreach (Parallax parallax in parallaxes)
            {
                parallax.StopMoving();
            }
        }
        
        public void SetSpeed(float newSpeed)
        {
            foreach (Parallax parallax in parallaxes)
            {
                parallax.SetSpeed(newSpeed);
            }
        }
    }
}
