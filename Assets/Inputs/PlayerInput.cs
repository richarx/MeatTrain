using Locomotor;
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
        }

        private void OnPressInput()
        {
            GreatLocomotor.instance.AddSpeed();
        }
    }
}
