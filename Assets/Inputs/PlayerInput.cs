using Locomotor;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                OnPressInput();

            if (Keyboard.current.rKey.isPressed)
                OnSlowInput();
            
            if (Keyboard.current.fKey.wasPressedThisFrame)
                OnHonkInput();
        }

        private void OnPressInput()
        {
            GreatLocomotor.instance.AddSpeed();
        }

        private void OnSlowInput()
        {
            GreatLocomotor.instance.DecreaseSpeed();
        }

        private void OnHonkInput()
        {
            GreatLocomotor.instance.Honk();
        }
    }
}
