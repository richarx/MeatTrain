using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [SerializeField] private AudioClip spookyAmbience;

    [SerializeField] private AudioClip ambientSoundWhale;
    [SerializeField] private AudioClip ambientSoundExplosion;

    private AudioSource currentAmbientSound;

    void Start()
    {
        StartCoroutine(WhaleNoiseLoop());
        StartCoroutine(ExplosionAmbianceSound());

        PlayLoopSound();
    }

    private IEnumerator WhaleNoiseLoop()
    {
        while (true)
        {
            float randomInterval = Random.Range(30f, 120f);
            
            if (currentAmbientSound == null)
            {
                currentAmbientSound = SFXManager.Instance.PlaySFX(ambientSoundWhale);
                yield return new WaitWhile(() => currentAmbientSound != null && currentAmbientSound.isPlaying);
                currentAmbientSound = null;
            }
            
            yield return new WaitForSeconds(randomInterval);
        }
    }

    private IEnumerator ExplosionAmbianceSound()
    {
        while (true)
        {
            float randomInterval = Random.Range(30f, 120f);

            if (currentAmbientSound == null)
            {
                currentAmbientSound = SFXManager.Instance.PlaySFX(ambientSoundExplosion);
                yield return new WaitWhile(() => currentAmbientSound != null && currentAmbientSound.isPlaying);
                currentAmbientSound = null;
            }

            yield return new WaitForSeconds(randomInterval);
        }
    }

    private void PlayLoopSound()
    {
        SFXManager.Instance.PlaySFX(spookyAmbience, loop: true);
    }
}   

