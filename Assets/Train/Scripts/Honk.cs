using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class Honk : MonoBehaviour
{
    public static UnityEvent OnHonk = new UnityEvent();

    [SerializeField] private AudioClip honk;
    [SerializeField] private GameObject bloodGeyserParticle;
    [SerializeField] private List<GameObject> permanentBloodSplatters;
    [SerializeField] private float animationDuration;

    private bool isHonking;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isHonking == false)
            StartCoroutine(OnHonkInput());
    }
    private IEnumerator OnHonkInput()
    {
        isHonking = true;

        OnHonk.Invoke();

        SFXManager.Instance.PlaySFX(honk);

        bloodGeyserParticle.SetActive(true);
        
        StartCoroutine(SpawnPermanentBloodSplatter());

        yield return new WaitForSeconds(animationDuration);
        bloodGeyserParticle.SetActive(false);

        isHonking = false;
    }

    private IEnumerator SpawnPermanentBloodSplatter()
    {
        float timer = 0f;

        while (timer < animationDuration)
        {
            SpawnOneBloodSplatter();
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }
    }

    private void SpawnOneBloodSplatter()
    {
        int whichSplatter = Random.Range(0, permanentBloodSplatters.Count);

        float randomHorizontalOffset = Random.Range(-1f, 1f); // x : 0.75 initial
        float randomVerticalOffset = -1f - Random.Range(0, 1f); //y : 1.7 initial

        float whichPositionX = transform.position.x + randomHorizontalOffset;
        float whichPositionY = transform.position.y + randomVerticalOffset;

        Vector2 whichPosition = new Vector2(whichPositionX, whichPositionY);

        GameObject splatter = Instantiate(permanentBloodSplatters[whichSplatter], whichPosition, Quaternion.identity);

        SqueezeAndStretch splatterSqueeze = splatter.GetComponent<SqueezeAndStretch>();
        if (splatterSqueeze != null)
            splatterSqueeze.Trigger();

        EntitySpawner.instance.AddNewPersistentEntity(splatter, permanentBloodSplatters[whichSplatter]);
    }
}
