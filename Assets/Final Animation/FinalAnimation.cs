using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Final_Animation
{
    public class FinalAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject blackScreen;
        
        [Space]
        [SerializeField] private Image planet;
        [SerializeField] private float minPlanetScale;
        [SerializeField] private float planetScaleDuration;
        
        [Space]
        [SerializeField] private Image face;
        [SerializeField] private float minFaceScale;
        [SerializeField] private float faceScaleDuration;
        [SerializeField] private float faceMoveDistance;
        [SerializeField] private float faceMoveDuration;
        
        [Space]
        [SerializeField] private AudioClip textureSound;
        [SerializeField] private float textureSoundFadeDuration;
        [SerializeField] private float textureSoundMaxVolume;
        
        [Space]
        [SerializeField] private Image whiteScreen;
        [SerializeField] private TextMeshProUGUI finalText;

        public static FinalAnimation instance;

        private void Awake()
        {
            instance = this;
        }

        public void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                blackScreen.SetActive(true);
                TriggerFinalAnimation();
            }
        }

        public void TriggerFinalAnimation()
        {
            StartCoroutine(FinalAnimationCoroutine());
        }

        private IEnumerator FinalAnimationCoroutine()
        {
            AudioSource source = SFXManager.Instance.PlaySFX(textureSound, volume:0.0f);
            yield return FadeInSound(source);
            
            planet.gameObject.SetActive(true);
            StartCoroutine(PlanetScaler());
            yield return Tools.Tools.Fade(planet, planetScaleDuration * 0.5f, true);
            StartCoroutine(Tools.Tools.Fade(planet, planetScaleDuration * 0.5f, false));

            Transform faceTarget = face.transform.parent;
            faceTarget.gameObject.SetActive(true);
            StartCoroutine(Tools.Tools.Fade(face, faceScaleDuration * 3.0f, true));
            yield return FaceScaler();

            Vector3 position = faceTarget.position;
            position.y -= faceMoveDistance;
            face.GetComponent<Animator>().Play("CloseMouth");
            StartCoroutine(Tools.Tools.TweenPosition(faceTarget, position.x, position.y, faceMoveDuration));
            yield return new WaitForSeconds(faceMoveDuration);

            whiteScreen.gameObject.SetActive(true);
            yield return Tools.Tools.Fade(whiteScreen, 4.0f, true);
            faceTarget.gameObject.SetActive(false);
            planet.gameObject.SetActive(false);
            yield return Tools.Tools.Fade(whiteScreen, 1.0f, false);
            whiteScreen.gameObject.SetActive(false);
            
            finalText.gameObject.SetActive(true);
            yield return Tools.Tools.Fade(finalText, 3.0f, true);
            
            Time.timeScale = 0.0f;
        }

        private IEnumerator FadeInSound(AudioSource source)
        {
            float duration = textureSoundFadeDuration;
            float timer = 0.0f;
            while (timer <= duration)
            {
                source.volume = Tools.Tools.NormalizeValueInRange(timer, 0.0f, duration, 0.0f, textureSoundMaxVolume);
                yield return null;
                timer += Time.deltaTime;
            }
        }

        private IEnumerator FaceScaler()
        {
            Transform target = face.transform.parent;

            float timer = faceScaleDuration;
            while (timer >= 0.0f)
            {
                float scale = Tools.Tools.NormalizeValueInRange(timer, 0.0f, faceScaleDuration, minFaceScale, 1.0f);
                target.localScale = new Vector3(scale, scale, scale);
                yield return null;
                timer -= Time.deltaTime;
            }
        }

        private IEnumerator PlanetScaler()
        {
            float timer = planetScaleDuration;
            while (timer >= 0.0f)
            {
                float scale = Tools.Tools.NormalizeValueInRange(timer, 0.0f, planetScaleDuration, minPlanetScale, 1.0f);
                planet.transform.localScale = new Vector3(scale, scale, scale);
                yield return null;
                timer -= Time.deltaTime;
            }
        }
    }
}
