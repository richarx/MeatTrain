using System;
using Dialog;
using Drag_and_drop;
using Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class Speakable : MonoBehaviour
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float chancesToSpeak;
        [SerializeField] private Vector2 spawnDialogOffset;
        [SerializeField] private NPCType npc;

        private bool shouldSpeak;
        private bool isSpeaking;
        private float spawnPosition;

        private DialogBox dialogBox;
        
        private void Start()
        {
            shouldSpeak = Random.Range(0.0f, 1.0f) <= chancesToSpeak;
            spawnPosition = Random.Range(7.0f, 8.0f);

            Draggable draggable = GetComponent<Draggable>();
            if (draggable != null)
                draggable.OnDrag.AddListener(HideDialog);
        }

        private void Update()
        {
            if (shouldSpeak && !isSpeaking && transform.position.x <= spawnPosition)
                Speak();
        }

        private void Speak()
        {
            dialogBox = DialogManager.instance.SpawnDialog(transform.position + spawnDialogOffset.ToVector3(), npc);
            isSpeaking = true;
        }

        private void HideDialog()
        {
            if (dialogBox != null)
            {
                dialogBox.HideAndDestroy();
                dialogBox = null;
            }
        }
    }
}
