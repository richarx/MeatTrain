using System;
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

        private void Start()
        {
            Instance = this;  
        }

        private void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
                ChangeLevel();
        }

        public void ChangeLevel()
        {
            currentLevel += 1;
            OnLevelChange.Invoke(currentLevel);
        }
    }
}
