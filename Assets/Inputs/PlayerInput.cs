using Locomotor;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        private bool isNextAny = true;
        private bool isNextLeft;

        private void Update()
        {
            bool left = CheckLeftInput();
            bool right = CheckRightInput();

            if (isNextAny || (left && isNextLeft) || (right && !isNextLeft))
            {
                if (isNextAny)
                {
                    isNextAny = false;
                    isNextLeft = left;
                }

                OnPressInput();
            }

            if (Keyboard.current.rKey.isPressed)
                OnSlowInput();
        }

        private void OnPressInput()
        {
            isNextLeft = !isNextLeft;

            GreatLocomotor.instance.AddSpeed();
        }

        private bool CheckLeftInput()
        {
            return Keyboard.current.qKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame;
        }

        private bool CheckRightInput()
        {
            return Keyboard.current.dKey.wasPressedThisFrame;
        }

        private void OnSlowInput()
        {
            GreatLocomotor.instance.DecreaseSpeed();
        }
    }
}
