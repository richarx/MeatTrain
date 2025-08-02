using System.Collections.Generic;
using Locomotor;
using UnityEngine;

namespace Parallax
{
    public class ParallaxManager : MonoBehaviour
    {
        [SerializeField] private float defaultSpeed;
        [SerializeField] private List<Parallax> parallaxes;

        private void Start()
        {
            GreatLocomotor.OnUpdateSpeed.AddListener(SetSpeed);
            
            foreach (Parallax parallax in parallaxes)
            {
                parallax.SetSpeed(defaultSpeed);
            }
        }

        private void SetSpeed(float newSpeed)
        {
            foreach (Parallax parallax in parallaxes)
            {
                parallax.SetSpeed(newSpeed);
            }
        }
    }
}
