using System.Collections;
using TMPro;
using UnityEngine;

namespace VFX.Meat_Score
{
    public class MeatScore : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private SpriteRenderer plus;
        [SerializeField] private SpriteRenderer icon;
        
        [Space]
        [SerializeField] private float animationDuration;
        [SerializeField] private float animationDistance;
        
        private SqueezeAndStretch squeezeAndStretch;
        
        private bool isSetup;
        
        public void Setup(float score)
        {
            if (isSetup)
                return;

            squeezeAndStretch = GetComponent<SqueezeAndStretch>();
            
            text.text = $"{Mathf.FloorToInt(score)}";
            isSetup = true;

            StartCoroutine(AnimateAndDestroy());
        }

        private IEnumerator AnimateAndDestroy()
        {
            Vector3 target = transform.position + Vector3.down * animationDistance;
            
            StartCoroutine(Tools.Tools.Fade(text, 0.1f, true));
            StartCoroutine(Tools.Tools.Fade(plus, 0.1f, true));
            yield return Tools.Tools.Fade(icon, 0.1f, true);
            squeezeAndStretch.Trigger();
            
            StartCoroutine(Tools.Tools.TweenPosition(transform, target.x, target.y, animationDuration));
            StartCoroutine(Tools.Tools.Fade(text, animationDuration, false));
            StartCoroutine(Tools.Tools.Fade(plus, animationDuration, false));
            StartCoroutine(Tools.Tools.Fade(icon, animationDuration, false));
            
            Destroy(gameObject, animationDuration);
        }
    }
}
