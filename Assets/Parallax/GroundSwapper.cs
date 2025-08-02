using System;
using MiniMap;
using UnityEngine;

namespace Parallax
{
    public class GroundSwapper : MonoBehaviour
    {
        [SerializeField] private Parallax targetParallax;
        [SerializeField] private Parallax groundParallax;
        [SerializeField] private Sprite forest;
        [SerializeField] private Sprite desert;
        [SerializeField] private Sprite city;

        private bool shouldSwap;
        
        private void Start()
        {
            Map.OnReachNewBiome.AddListener(() => shouldSwap = true);
            targetParallax.OnReachExit.AddListener(() =>
            {
                if (shouldSwap)
                {
                    shouldSwap = false;
                    SwapGround();
                }
            });
            groundParallax.OnReachExit.AddListener(() =>
            {
                groundParallax.StopMoving();
            });
            groundParallax.StopMoving();
        }

        private void SwapGround()
        {
            Map.Biome biome = Map.instance.currentBiome;
            
            groundParallax.SetNewSprite(SelectSprite(biome), true);
            groundParallax.StartMoving();
        }
        
        private Sprite SelectSprite(Map.Biome biome)
        {
            switch (biome)
            {
                case Map.Biome.Forest:
                    return forest;
                case Map.Biome.Desert:
                    return desert;
                case Map.Biome.City:
                    return city;
                default:
                    throw new ArgumentOutOfRangeException(nameof(biome), biome, null);
            }
        }
    }
}
