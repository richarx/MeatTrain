using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Honk : MonoBehaviour
{
    public static UnityEvent OnHonk = new UnityEvent();

    [SerializeField] private AudioClip honk;
    [SerializeField] private GameObject bloodGeyserParticle;
    [SerializeField] private float animationDuration;

    private bool isHonking;

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame && isHonking == false)
            StartCoroutine(OnHonkInput());
    }
    private IEnumerator OnHonkInput()
    {
        isHonking = true;

        OnHonk.Invoke();

        SFXManager.Instance.PlaySFX(honk);

        bloodGeyserParticle.SetActive(true);
        yield return new WaitForSeconds(animationDuration);
        bloodGeyserParticle.SetActive(false);

        isHonking = false;
    }

}
