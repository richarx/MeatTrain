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

            if (Keyboard.current.rKey.wasPressedThisFrame)
                OnSlowInput();

            if (Keyboard.current.rKey.wasReleasedThisFrame)
                OnSlowRelease();

            if (Keyboard.current.fKey.wasPressedThisFrame)
                OnHonkInput();
        }

        private void OnPressInput()
        {
            GreatLocomotor.instance.AddSpeed();
        }

        private void OnSlowInput()
        {
            GreatLocomotor.instance.isBreaking = true;
        }

        private void OnSlowRelease()
        {
            GreatLocomotor.instance.isBreaking = false;
        }

        private void OnHonkInput()
        {
            GreatLocomotor.instance.Honk();
        }
    }
}
