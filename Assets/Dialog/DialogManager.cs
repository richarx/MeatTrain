using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialog
{
    public enum NPCType
    {
        BlackDude,
        Fanatic,
        Rabbit
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
                case NPCType.Rabbit:
                    return rabbit;
                default:
                    throw new ArgumentOutOfRangeException(nameof(npcType), npcType, null);
            }   
        }

        string[] blackDude = new[]
        {
            "What is this thing ..?",
            "AAAAAHHH",
            "Oh NO!",
            "RUN",
            "Is this a metaphor for capitalism ?"
        };
        
        string[] fanatic = new[]
        {
            "REJOICE",
            "Vora'Ul look at me!",
            "ME ME ME",
            "Let me be meat",
            "Let all be meat",
            "Consume me !",
            "Choose me O Great One",
            "CHOO CHOO",
            "MASTER",
            "Is this a metaphor for capitalism ?"
        };

        string[] rabbit = new[]
        {
            "mee ?",
        };
    }
}
