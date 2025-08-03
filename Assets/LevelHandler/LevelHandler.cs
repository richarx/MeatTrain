using Locomotor;
using System;
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

        [SerializeField] private List<int> maxGrapPerLevel;
        [HideInInspector] public List<int> MaxGrapPerLevel => maxGrapPerLevel;

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private List<AudioClip> levelUpSounds;
        [SerializeField] private AudioClip evolvingSound;

        [SerializeField] private Image bloodOverlay;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            MeatWagon.OnMeatWagonFull.AddListener(DisplayToolTip);

            bloodOverlay.gameObject.SetActive(false);

            DisplayFirstLevelPrompt();
        }

        private void DisplayFirstLevelPrompt()
        {
            var tooltip = ToolTipManager.instance.DisplayToolTip("Press A and D to Move");

            int count = 0;
            bool displayFirstPopup = false;
            bool displaySecondPopup = false;
            bool displayThirdPopup = false;

            GreatLocomotor.OnAccelerate.AddListener(() =>
            {
                count++;

                if (count > 10 && displayFirstPopup == false)
                {
                    tooltip = ToolTipManager.instance.DisplayToolTip("Press R to brake");
                    displayFirstPopup = true;
                }
            });

            GreatLocomotor.OnBrake.AddListener(() => 
            {
                if (displaySecondPopup == false && displayFirstPopup == true)
                {
                    tooltip = ToolTipManager.instance.DisplayToolTip("Drag and drop food \nin the middle wagon");
                    displaySecondPopup = true;
                }
            });

            MeatWagon.OnStartEat.AddListener(() =>
            {
                if (displayFirstPopup == true && displaySecondPopup == true && displayThirdPopup == false)
                {
                    tooltip = ToolTipManager.instance.DisplayToolTip("Eat away !", 3.0f);
                    displayThirdPopup = true;
                }
            });
        }

        private void DisplayToolTip()
        {
            ToolTipManager.instance.DisplayToolTip($"Press Space to $Evolve$\n[{currentLevel}/6]");
        }

        private void Update()
        {
            if (MeatWagon.instance.isFull && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ToolTipManager.instance.DisplayToolTip(ComputeLevelUpText(), 2.0f);
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
            StartCoroutine(DisplayHonkPrompt());
        }

        private string ComputeLevelUpText()
        {
            string[] array = new[]
            {
                "More Meat",
                "Let all be Meat",
                "Where meat ?",
                "I eat therefore I AM",
                "Close the loop",
            };

            if (currentLevel > array.Length)
                return array[^1];
            
            return array[currentLevel - 1];
        }

        private void PlayLevelUpSound()
        {
            SFXManager.Instance.PlayRandomSFX(levelUpSounds);
            SFXManager.Instance.PlaySFX(evolvingSound);
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

        private IEnumerator DisplayHonkPrompt()
        {
            if (currentLevel != 3)
                yield break;

            yield return new WaitForSeconds(5.0f);
            ToolTipManager.instance.DisplayToolTip("Press Space to Honk", 3.0f);
        }
    }
}
