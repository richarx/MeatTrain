using System.Collections;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private SpriteRenderer box;
        
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
            yield return Tools.Tools.Fade(box, animationDuration, true, maxAlpha:0.2f);
            squeezeAndStretch.Trigger();
        }
    }
}
