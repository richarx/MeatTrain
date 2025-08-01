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
                ToolTipManager.instance.DisplayToolTip("You have reached a new Stage", 1.0f);
                ChangeLevel();
            }
        }

        private void ChangeLevel()
        {
            currentLevel += 1;
            OnLevelChange.Invoke(currentLevel);
        }
    }
}
