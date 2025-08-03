using System;
using Dialog;
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
        
        private void Start()
        {
            shouldSpeak = Random.Range(0.0f, 1.0f) <= chancesToSpeak;
            spawnPosition = Random.Range(7.0f, 8.0f);
        }

        private void Update()
        {
            if (shouldSpeak && !isSpeaking && transform.position.x <= spawnPosition)
                Speak();
        }

        private void Speak()
        {
            DialogManager.instance.SpawnDialog(transform.position + spawnDialogOffset.ToVector3(), npc);
            isSpeaking = true;
        }
    }
}
