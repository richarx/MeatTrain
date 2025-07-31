using UnityEngine;

namespace UI
{
    public class BlackBars : MonoBehaviour
    {
        [SerializeField] private GameObject blackBars;
        private void Start()
        {
            blackBars.SetActive(true);
        }
    }
}
