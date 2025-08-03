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
        [SerializeField] private Transform trainTail;
        

        public static UnityEvent OnTriggerEndScene = new UnityEvent();
        
        public static EndScene instance;
        
        private Camera mainCamera;
    
        private bool isTriggered;
        public bool IsTriggered => isTriggered;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;
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

        private float moveTailVelocity;
        private void MoveTrainTailTowardsTarget(float timer)
        {
            Vector3 current = trainTail.position;
            current.x = Mathf.SmoothDamp(current.x, 1.5f, ref moveTailVelocity, duration / 2.0f);
            trainTail.position = current;
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
