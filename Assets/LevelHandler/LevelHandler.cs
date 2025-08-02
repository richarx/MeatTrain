using System.Collections.Generic;
using Train.Eat_on_Collision;
using UI.ToolTip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LevelHandler
{
    public class LevelHandler : MonoBehaviour
    {
        public static LevelHandler Instance;

        public static UnityEvent<int> OnLevelChange = new UnityEvent<int>();

        private int currentLevel = 1;
        public int CurrentLevel => currentLevel;

        [SerializeField] private List<AudioClip> levelUpSounds;

        private void Start()
        {
            Instance = this;
            MeatWagon.OnMeatWagonFull.AddListener(DisplayToolTip);
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
        }

        private string ComputeLevelUpText()
        {
            string[] array = new[]
            {
                "More...",
                "Need More...",
                "More meat...",
                "Where meat ?..",
                "So Hungry...",
                "Not enough...",
            };

            if (currentLevel > array.Length)
                return array[^1];
            
            return array[currentLevel - 1];
        }

        private void PlayLevelUpSound()
        {
            SFXManager.Instance.PlayRandomSFX(levelUpSounds);
        }
    }
}
