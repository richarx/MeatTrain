using UnityEngine;

namespace Entities
{
    public class Spawnable : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;

        public GameObject GetPrefab()
        {
            return prefab;
        }
    }
}
