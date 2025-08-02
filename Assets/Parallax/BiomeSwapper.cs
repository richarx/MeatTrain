using System;
using MiniMap;
using UnityEngine;

namespace Parallax
{
    public class BiomeSwapper : MonoBehaviour
    {
        [SerializeField] private Parallax parallax;
        [SerializeField] private Sprite forest;
        [SerializeField] private Sprite desert;
        [SerializeField] private Sprite city;

        private void Start()
        {
            Map.OnReachNewBiome.AddListener(UpdateBiomeSprites);
        }

        private void UpdateBiomeSprites()
        {
            Map.Biome biome = Map.instance.currentBiome;
            parallax.SetNewSprite(SelectSprite(biome));
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
