using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private SpriteRenderer box;
        [SerializeField] private float fadeValue;
        
        [Space]
        [SerializeField] private float animationDuration;
        
        private SqueezeAndStretch squeezeAndStretch;
        
        private bool isSetup;
    
        public void Setup(string dialog)
        {
            if (isSetup)
                return;

            squeezeAndStretch = GetComponent<SqueezeAndStretch>();
            
            text.text = $"{dialog}";
            isSetup = true;

            StartCoroutine(Animate());
        }

        private void Update()
        {
            if (transform.position.x <= -15.0f)
                Destroy(gameObject);
        }

        private IEnumerator Animate()
        {
            StartCoroutine(Tools.Tools.Fade(text, animationDuration, true));
            yield return Tools.Tools.Fade(box, animationDuration, true, maxAlpha:fadeValue);
            squeezeAndStretch.Trigger();
        }

        public void HideAndDestroy()
        {
            StartCoroutine(Tools.Tools.Fade(text, animationDuration, false));
            StartCoroutine(Tools.Tools.Fade(box, animationDuration, false, maxAlpha:fadeValue));
            Destroy(gameObject, animationDuration + 0.15f);
        }
    }
}
