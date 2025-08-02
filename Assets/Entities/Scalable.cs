using System.Collections;
using UnityEngine;

namespace Entities
{
    public class Scalable : MonoBehaviour
    {
        [SerializeField] private float initialScale;
        [SerializeField] private float scaleFactorPerLevel;

        private Vector3 velocity;
        
        private void Start()
        {
            LevelHandler.LevelHandler.OnLevelChange.AddListener((level) => StartCoroutine(ScaleDownDuringLevelUp(level)));
            transform.localScale = ComputeTargetScale(LevelHandler.LevelHandler.Instance.CurrentLevel);
        }

        private IEnumerator ScaleDownDuringLevelUp(int level)
        {
            float duration = LevelHandler.LevelHandler.Instance.LevelUpAnimationDuration;

            Vector3 currentScale = transform.localScale;
            Vector3 targetScale = ComputeTargetScale(level);
            
            while (currentScale.magnitude > targetScale.magnitude)
            {
                currentScale = Vector3.SmoothDamp(currentScale, targetScale, ref velocity, duration);
                transform.localScale = currentScale;
                yield return null;
            }
        }

        private Vector3 ComputeTargetScale(int level)
        {
            return Vector3.one * (initialScale - (scaleFactorPerLevel * (level - 1)));
        }
    }
}
