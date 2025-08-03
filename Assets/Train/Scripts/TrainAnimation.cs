using End_Scene;
using Locomotor;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class TrainAnimation : MonoBehaviour
{
    [SerializeField] private List<Transform> trainPieces;

    [SerializeField] private float maxIntensity;

    [SerializeField] private AudioClip crawlingSound;
    [SerializeField] private AudioClip finalCrawlingSound;

    [SerializeField] private GameObject followingWagonManager;
    [SerializeField] private List<Transform> followingPieces;

    AudioSource currentCrawlingSound;

    private bool isLocked;

    void Start()
    {
        for (int i = 0; i < trainPieces.Count; i++)
        {
            StartCoroutine(Shake(trainPieces[i]));
        }

        currentCrawlingSound = SFXManager.Instance.PlaySFX(crawlingSound, 0f, loop: true);

        LevelHandler.LevelHandler.OnLevelChange.AddListener(ActivateNextPiece);

        EndScene.OnTriggerEndSceneBlackScreen.AddListener(TriggerFinalAnimation);
        EndScene.OnTriggerEndScene.AddListener(UpdateCrawlingSound);
    }

    private void Update()
    {
        if (isLocked)
            return;

        currentCrawlingSound.volume = Mathf.Clamp(Tools.Tools.NormalizeValueInRange(GreatLocomotor.instance.CurrentSpeed, 0, 2.0f, 0, 0.5f), 0, 0.5f);
    }

    private IEnumerator Shake(Transform target)
    {
        while (true)
        {
            float intensity = Tools.Tools.NormalizeValueInRange(GreatLocomotor.instance.CurrentSpeed, 0, 2, 0, maxIntensity);
            
            Vector2 previousShake = Vector2.zero;
            float timer = 0.0f;

            Vector2 direction = Random.insideUnitCircle;

            target.position += ((direction * intensity) - previousShake).ToVector3();
            previousShake = direction * intensity;

            yield return null;
            timer += Time.deltaTime;

            target.position -= previousShake.ToVector3();
        }
    }

    private void ActivateNextPiece(int level)
    {
        if (level != 2)
            return;

        followingWagonManager.SetActive(true);

        for (int i = 0; i < followingPieces.Count; i++)
        {
            StartCoroutine(ShakeFollowingPieces(followingPieces[i]));
        }
    }

    private IEnumerator ShakeFollowingPieces(Transform target)
    {
        while (true)
        {
            float intensity = Tools.Tools.NormalizeValueInRange(GreatLocomotor.instance.CurrentSpeed, 0, 2, 0, maxIntensity);

            Vector2 previousShake = Vector2.zero;
            float timer = 0.0f;

            Vector2 direction = Random.insideUnitCircle;

            target.position += ((direction * intensity) - previousShake).ToVector3();
            previousShake = direction * intensity;

            yield return null;
            timer += Time.deltaTime;

            target.position -= previousShake.ToVector3();
        }
    }

    private void TriggerFinalAnimation()
    {
        if (currentCrawlingSound != null)
            currentCrawlingSound.Stop();
    }

    private void UpdateCrawlingSound()
    {
        isLocked = true;

        currentCrawlingSound.Stop();
        currentCrawlingSound = null;
        currentCrawlingSound = SFXManager.Instance.PlaySFX(finalCrawlingSound, volume: 0.1f, loop: true);
    }
}
