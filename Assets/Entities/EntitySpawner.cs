using System;
using System.Collections;
using System.Collections.Generic;
using MiniMap;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class PersistentEntity
    {
        public GameObject prefab;
        public float distanceFromStart;
        [Range(0.0f, 1.0f)] public float heightOnScreenPercent;
        [HideInInspector] public GameObject instance;
        public bool isSpawned => instance != null;
    }
    
    public class EntitySpawner : MonoBehaviour
    {
        [SerializeField] List<PersistentEntity> entityPrefabs;

        public static EntitySpawner instance;

        private Camera mainCamera;
        
        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            mainCamera = Camera.main;
            InitializeScene();
            while (true)
            {
                CheckForEntitiesToSpawn();
                yield return null;
            }
        }
        
        public void AddNewPersistentEntity(GameObject currentGameObject)
        {
            PersistentEntity entity = new PersistentEntity();

            entity.prefab = currentGameObject.GetComponent<Spawnable>().GetPrefab();
            Vector3 position = currentGameObject.transform.position;
            entity.distanceFromStart = ComputeDistanceFromStart(position.x);
            entity.heightOnScreenPercent = ComputeHeightPercent(position.y);
            entity.instance = currentGameObject;
        }

        private float ComputeDistanceFromStart(float positionX)
        {
            float position = Map.instance.ComputePositionOnPlanet();
            return position + positionX;
        }

        private void InitializeScene()
        {
            foreach (PersistentEntity entity in entityPrefabs)
            {
                if (entity.distanceFromStart >= -15.0f && entity.distanceFromStart <= 17.5f)
                    Spawn(entity, 0.0f);
            }
        }

        private void CheckForEntitiesToSpawn()
        {
            float positionOnPlanet = Map.instance.ComputePositionOnPlanet(); //206 => 214
            float positionDelta = (positionOnPlanet + 15.0f) % Map.instance.WorldSize; //221 % 210 => 11

            for (int i = 0; i < entityPrefabs.Count; i++)
            {
                if (entityPrefabs[i].isSpawned)
                {
                    if (CheckIfOutOfScreen(entityPrefabs[i]))
                        entityPrefabs[i] = HideEntity(entityPrefabs[i]);
                    continue;
                }

                if (entityPrefabs[i].distanceFromStart >= positionDelta - 2.5f && entityPrefabs[i].distanceFromStart <= positionDelta + 2.5f)
                    entityPrefabs[i] = Spawn(entityPrefabs[i], positionOnPlanet);
            }
        }

        private bool CheckIfOutOfScreen(PersistentEntity entity)
        {
            return entity.instance.transform.position.x <= -20.0f;
        }

        private PersistentEntity HideEntity(PersistentEntity entity)
        {
            Destroy(entity.instance);
            entity.instance = null;
            return entity;
        }

        private PersistentEntity Spawn(PersistentEntity entity, float positionOnPlanet)
        {
            float x = entity.distanceFromStart - positionOnPlanet;
            if (x < 0.0f)
                x += Map.instance.WorldSize;
            
            float y = ComputeHeightPosition(entity.heightOnScreenPercent);
            Vector3 position = new Vector3(x, y, 0.0f);
            GameObject spawn = Instantiate(entity.prefab, position, Quaternion.identity);
            entity.instance = spawn;

            return entity;
        }

        private float ComputeHeightPosition(float heightPercent)
        {
            float screenPosition = Screen.height * heightPercent;
            
            return mainCamera.ScreenToWorldPoint(Vector3.up * screenPosition).y;
        }

        private float ComputeHeightPercent(float heightPosition)
        {
            float height = mainCamera.WorldToScreenPoint(Vector3.up * heightPosition).y;

            return Screen.height / height;
        }
    }
}
