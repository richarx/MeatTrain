using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private Squashable squashable;
    private Digestable digestable;

    [SerializeField] private List<GameObject> bloodParticles;
    [SerializeField] private Vector2 bloodParticleSpawnPositionOffset;

    [SerializeField] private List<GameObject> greenMucusParticles;
    [SerializeField] private Vector2 greenMucusSpawnPositionOffset;

    void Start()
    {
        squashable = GetComponent<Squashable>();
        digestable = GetComponent<Digestable>();

        if (digestable != null)
            digestable.OnIsDigested.AddListener(GreenMucusVFX);

        if (squashable != null)
            squashable.OnSquash.AddListener(BloodVFX);
    }

    private void BloodVFX()
    {
        if (bloodParticles == null)
            return;

        Vector2 spawnPosition = (Vector2)transform.position + bloodParticleSpawnPositionOffset;
        int whichParticle = Random.Range(0, bloodParticles.Count);

        Instantiate(bloodParticles[whichParticle], spawnPosition, Quaternion.identity);
    }

    private void GreenMucusVFX()
    {
        if (greenMucusParticles == null)
            return;

        Vector2 spawnPosition = (Vector2)transform.position + greenMucusSpawnPositionOffset;
        int whichParticle = Random.Range(0, greenMucusParticles.Count);

        Instantiate(greenMucusParticles[whichParticle], spawnPosition, Quaternion.identity);
    }
}
