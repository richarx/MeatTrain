using Locomotor;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class TrainAnimation : MonoBehaviour
{
    [SerializeField] private List<Transform> trainPieces;
    [SerializeField] private float maxIntensity;

    void Start()
    {
        for (int i = 0; i < trainPieces.Count; i++)
        {
            StartCoroutine(Shake(trainPieces[i]));
        }
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
}
