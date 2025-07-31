using System;
using TMPro;
using UnityEngine;

namespace Locomotor
{
    public class SpeedDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentSpeedText;
        [SerializeField] private TextMeshProUGUI distanceText;

        private void Start()
        {
            GreatLocomotor.OnUpdateSpeed.AddListener(UpdateTexts);
            UpdateTexts(GreatLocomotor.instance.CurrentSpeed);
        }

        private void Update()
        {
            distanceText.text = $"Distance : {GreatLocomotor.instance.DistanceCrawled:F}";
        }

        private void UpdateTexts(float speed)
        {
            currentSpeedText.text = $"Current Speed : {speed:F}";
        }
    }
}
