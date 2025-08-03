using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialog
{
    public enum NPCType
    {
        BlackDude,
        Fanatic
    }
    
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private DialogBox dialogBoxPrefab;
        
        public static DialogManager instance;

        private void Awake()
        {
            instance = this;
        }

        public DialogBox SpawnDialog(Vector3 position, NPCType npcType)
        {
            DialogBox box = Instantiate(dialogBoxPrefab, position, Quaternion.identity);
            box.Setup(ComputeText(npcType));
            return box;
        }

        private string ComputeText(NPCType npcType)
        {
            string[] array = SelectArray(npcType);

            int index = Random.Range(0, array.Length);
            
            return array[index];
        }

        private string[] SelectArray(NPCType npcType)
        {
            switch (npcType)
            {
                case NPCType.BlackDude:
                    return blackDude;
                case NPCType.Fanatic:
                    return fanatic;
                default:
                    throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null);
            }   
        }

        string[] blackDude = new[]
        {
            "more...",
            "need more...",
            "more meat...",
            "where meat ?..",
            "So hungry...",
            "not enough...",
        };
        
        string[] fanatic = new[]
        {
            "more...",
            "need more...",
            "more meat...",
            "where meat ?..",
            "So hungry...",
            "not enough...",
        };
    }
}
