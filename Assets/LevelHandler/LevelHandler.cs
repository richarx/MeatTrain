using System.Collections;
using System.Collections.Generic;
using Train.Eat_on_Collision;
using UI.ToolTip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LevelHandler
{
    public class LevelHandler : MonoBehaviour
    {
        public static LevelHandler Instance;

        public static UnityEvent<int> OnLevelChange = new UnityEvent<int>();

        private int currentLevel = 1;
        public int CurrentLevel => currentLevel;
        
        private float levelUpAnimationDuration = 1.5f;
        public float LevelUpAnimationDuration => levelUpAnimationDuration;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private List<AudioClip> levelUpSounds;

        [SerializeField] private Image bloodOverlay;

        private void Start()
        {
            Instance = this;
            MeatWagon.OnMeatWagonFull.AddListener(DisplayToolTip);

            bloodOverlay.gameObject.SetActive(false);
        }

        private void DisplayToolTip()
        {
            ToolTipManager.instance.DisplayToolTip("Press L to $Evolve$");
        }

        private void Update()
        {
            if (MeatWagon.instance.isFull && Keyboard.current.lKey.wasPressedThisFrame)
            {
                ToolTipManager.instance.DisplayToolTip(ComputeLevelUpText(), 1.0f);
                ChangeLevel();
            }
        }

        private void ChangeLevel()
        {
            currentLevel += 1;
            OnLevelChange.Invoke(currentLevel);
            PlayLevelUpSound();
            ScreenShake();
            StartCoroutine(DisplayOverlay());
        }

        private string ComputeLevelUpText()
        {
            string[] array = new[]
            {
                "more...",
                "need more...",
                "more meat...",
                "where meat ?..",
                "So hungry...",
                "not enough...",
            };

            if (currentLevel > array.Length)
                return array[^1];
            
            return array[currentLevel - 1];
        }

        private void PlayLevelUpSound()
        {
            SFXManager.Instance.PlayRandomSFX(levelUpSounds);
        }

        private void ScreenShake()
        {
            StartCoroutine(Tools.Tools.Shake(cameraTransform, 0.2f, 0.3f, true));
        }

        private IEnumerator DisplayOverlay()
        {
            bloodOverlay.gameObject.SetActive(true);

            yield return Tools.Tools.Fade(bloodOverlay, 0.3f, true, 0, 0.3f);
            yield return Tools.Tools.Fade(bloodOverlay, 0.3f, false, 0, 0.3f);

            bloodOverlay.gameObject.SetActive(false);
        }
    }
}
