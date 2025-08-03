using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace End_Scene
{
    public class EndScene : MonoBehaviour
    {
        [SerializeField] private int levelToTriggerEndScene;
        [SerializeField] private float duration;

        public static UnityEvent OnTriggerEndScene = new UnityEvent();
        
        public static EndScene instance;
    
        private bool isTriggered;
        public bool IsTriggered => isTriggered;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            LevelHandler.LevelHandler.OnLevelChange.AddListener((level) =>
            {
                if (!isTriggered && level >= levelToTriggerEndScene)
                    TriggerEndScene();
            });
        }

        private void Update()
        {
            if (!isTriggered && Keyboard.current.kKey.wasPressedThisFrame)
                TriggerEndScene();
        }

        private void TriggerEndScene()
        {
            if (isTriggered)
                return;
            
            OnTriggerEndScene?.Invoke();
            StartCoroutine(EndSceneCoroutine());
        }

        private IEnumerator EndSceneCoroutine()
        {
            float timer = 0.0f;
            while (timer <= duration)
            {
                MoveCameraTowardsTarget(timer);
                MoveTrainTailTowardsTarget(timer);
                SlowDownTime(timer);
                ShakeEverything(timer);
                yield return null;
                timer += Time.deltaTime;
            }

            Time.timeScale = 1.0f;
            ActivateBlackScreen();
            PlaySpookySound();
            yield return new WaitForSeconds(2.5f);
            TriggerFinalAnimation();
        }

        private void MoveCameraTowardsTarget(float timer)
        {
            
        }

        private void MoveTrainTailTowardsTarget(float timer)
        {
            
        }

        private void SlowDownTime(float timer)
        {
            
        }

        private void ShakeEverything(float timer)
        {
            
        }

        private void ActivateBlackScreen()
        {
            
        }

        private void PlaySpookySound()
        {
            
        }

        private void TriggerFinalAnimation()
        {
            
        }
    }
}
