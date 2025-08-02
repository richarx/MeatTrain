using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities
{
    [Serializable]
    public class MultipleEntity
    {
        public GameObject prefab;

        [FormerlySerializedAs("levelRequiredToSpawn")] [Space] 
        public int requiredLevelToSpawn = 1;
        
        [Space]
        public int minCount;
        public int maxCount;

        [Space] 
        public float minDistanceFromStart;
        public float maxDistanceFromStart;
        
        [Space] 
        [Range(0.0f, 1.0f)] public float minHeightOnScreen;
        [Range(0.0f, 1.0f)] public float maxHeightOnScreen;
    }
    
    public class EntityWorldFiller : MonoBehaviour
    {
        [SerializeField] private List<MultipleEntity> entitiesToFillWorld;
        
        private EntitySpawner entitySpawner;
        
        private void Awake()
        {
            entitySpawner = GetComponent<EntitySpawner>();
            LevelHandler.LevelHandler.OnLevelChange.AddListener(FillWorld);
            
            FillWorld(1);
        }

        private void FillWorld(int level)
        {
            foreach (MultipleEntity entity in entitiesToFillWorld)
            {
                if (entity.requiredLevelToSpawn == level)
                    AddEntitiesToWorldList(entity);
            }
        }

        private void AddEntitiesToWorldList(MultipleEntity entity)
        {
            int count = Random.Range(entity.minCount, entity.maxCount);

            for (int i = 0; i < count; i++)
            {
                entitySpawner.AddNewPersistentEntity(entity.prefab, ComputePosition(entity));
            }
        }

        private Vector2 ComputePosition(MultipleEntity entity)
        {
            float x = Random.Range(entity.minDistanceFromStart, entity.maxDistanceFromStart);
            float y = Random.Range(entity.minHeightOnScreen, entity.maxHeightOnScreen);
            return new Vector2(x, y);
        }
    }
}
