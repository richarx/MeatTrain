using Locomotor;
using UnityEngine;

namespace MiniMap
{
    public class Map : MonoBehaviour
    {
        public enum Biome
        {
            Forest,
            Desert,
            City
        }

        [SerializeField] private float worldSize;

        public static Map instance;
    
        private int currentBiomeIndex = 0;
        public Biome currentBiome => (Biome)currentBiomeIndex;

        public float WorldSize => worldSize;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (HasReachNewBiome())
                TriggerNextBiome();
        }

        private bool HasReachNewBiome()
        {
            float targetDistance = ComputeNextBiomeDistance();
            float currentPosition = ComputePositionOnPlanet();

            if (targetDistance < 1)
                return currentPosition < worldSize / 3.0f;
            
            return currentPosition > targetDistance;
        }

        private void TriggerNextBiome()
        {
            currentBiomeIndex += 1;
            if (currentBiomeIndex > 2)
                currentBiomeIndex = 0;
                
            Debug.Log($"Reached new Biome : {currentBiome}");
        }

        private float ComputeNextBiomeDistance()
        {
            int nextBiome = currentBiomeIndex + 1;
            if (nextBiome > 2)
                nextBiome = 0;
            
            return (worldSize / 3.0f) * nextBiome;
        }

        public float ComputePositionOnPlanet()
        {
            return GreatLocomotor.instance.DistanceCrawled % worldSize;
        }
    }
}
