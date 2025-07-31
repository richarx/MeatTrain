using System;
using System.Collections.Generic;
using UnityEngine;

namespace Parallax
{
    public class ParallaxManager : MonoBehaviour
    {
        [SerializeField] private float defaultSpeed;
        [SerializeField] private List<Parallax> parallaxes;

        private float previousSpeed;
        
        public static ParallaxManager instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            foreach (Parallax parallax in parallaxes)
            {
                parallax.SetSpeed(defaultSpeed);
            }

            previousSpeed = defaultSpeed;
        }

        private void Update()
        {
            if (Math.Abs(defaultSpeed - previousSpeed) > 0.01f)
            {
                previousSpeed = defaultSpeed;
                SetSpeed(defaultSpeed);
            }
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
