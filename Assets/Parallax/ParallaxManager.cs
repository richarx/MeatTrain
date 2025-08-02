using System.Collections.Generic;
using Locomotor;
using MiniMap;
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
            Map.OnReachNewBiome.AddListener(() => UpdateBiomeParallaxes(false));
            
            SetSpeed(defaultSpeed);
            UpdateBiomeParallaxes(true);
        }
        
        private void UpdateBiomeParallaxes(bool isInstant)
        {
            Map.Biome biome = Map.instance.currentBiome;

            foreach (Parallax parallax in parallaxes)
            {
                if (parallax.currentBiome != Map.Biome.None)
                    parallax.SetState(biome == parallax.currentBiome, isInstant);
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
