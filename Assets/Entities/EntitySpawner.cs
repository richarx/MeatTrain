using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> entityPrefabs;
    [SerializeField] List<Transform> spawnPositions;

    [SerializeField] private float spawnInterval;
    
    void Start()
    {
        //Listener sur le changement de biôme -> On va chercher la liste d'entités correspondante
        //Listener sur le nombre de boucle/distance parcourue -> On va updater l'intervalle de Spawn / peut-être liste d'entités

        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Spawn()
    {
        int whichSpawnPosition = Random.Range(0, spawnPositions.Count);
        int whichEntity = Random.Range(0, entityPrefabs.Count);

        GameObject entityTemp = Instantiate(entityPrefabs[whichEntity], spawnPositions[whichSpawnPosition].position, Quaternion.identity);
    }
}
