using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSounds : MonoBehaviour
{
    [SerializeField] private AudioClip ambientSound;

    void Start()
    {
        StartCoroutine(WhaleNoiseLoop());    
    }

    private IEnumerator WhaleNoiseLoop()
    {
        while (true)
        {
            float randomInterval = Random.Range(30f, 120f);
            SFXManager.Instance.PlaySFX(ambientSound);
            yield return new WaitForSeconds(randomInterval);
        }
    }
}
