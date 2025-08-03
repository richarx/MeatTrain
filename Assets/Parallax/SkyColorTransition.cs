using System;
using System.Collections;
using UnityEngine;

namespace Parallax
{
    public class SkyColorTransition : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sky;
        [SerializeField] private Gradient gradient;

        private void Start()
        {
            LevelHandler.LevelHandler.OnLevelChange.AddListener(UpdateSkyColor);
        }

        private void UpdateSkyColor(int level)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateSkyColorCoroutine(level));
        }

        private IEnumerator UpdateSkyColorCoroutine(int level)
        {
            Color current = gradient.Evaluate(Tools.Tools.NormalizeValue(level - 1, 0, 6));
            Color target = gradient.Evaluate(Tools.Tools.NormalizeValue(level, 0, 6));
            Color newColor = current;

            float timer = 0.0f;
            while (timer <= 1.5f)
            {
                newColor.r = Tools.Tools.NormalizeValueInRange(timer, 0.0f, 1.5f, current.r, target.r);
                newColor.g = Tools.Tools.NormalizeValueInRange(timer, 0.0f, 1.5f, current.g, target.g);
                newColor.b = Tools.Tools.NormalizeValueInRange(timer, 0.0f, 1.5f, current.b, target.b);
                sky.color = newColor;
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }
}
