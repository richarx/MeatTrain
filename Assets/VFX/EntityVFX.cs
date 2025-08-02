using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace VFX
{
    public class EntityVFX : MonoBehaviour
    {
        private Squashable squashable;
        private Digestable digestable;

        [SerializeField] private List<GameObject> bloodParticles;
        [SerializeField] private Vector2 bloodParticleSpawnPositionOffset;

        [SerializeField] private List<GameObject> greenMucusParticles;
        [SerializeField] private Vector2 greenMucusSpawnPositionOffset;

        [SerializeField] private List<GameObject> topSplatterParticles;
        [SerializeField] private List<GameObject> bottomSplatterParticles;

        void Start()
        {
            squashable = GetComponent<Squashable>();
            digestable = GetComponent<Digestable>();

            if (digestable != null)
                digestable.OnIsDigested.AddListener(GreenMucusVFX);

            if (squashable != null)
            {
                squashable.OnSquash.AddListener(BloodVFX);
                squashable.OnSquash.AddListener(CreatePermanentSplatter);
            }
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

        private void CreatePermanentSplatter()
        {
            bool isTop = Tools.Tools.RandomBool();

            float topSplatterPositionY = 0.7f; //hauteur du rail
            float bottomSplatterPositionY = Random.Range(0.1f, 0.45f); //hauteur random en dessous

            int whichSplatter = Random.Range(0, isTop ? topSplatterParticles.Count : bottomSplatterParticles.Count);

            Vector2 spawnPosition = new Vector2(transform.position.x, isTop ? topSplatterPositionY : bottomSplatterPositionY);

            GameObject splatterPrefab = isTop ? topSplatterParticles[whichSplatter] : bottomSplatterParticles[whichSplatter];

            GameObject splatter = Instantiate(splatterPrefab, spawnPosition, Quaternion.identity);
            
            EntitySpawner.instance.AddNewPersistentEntity(splatter, splatterPrefab);
        }
    }
}
