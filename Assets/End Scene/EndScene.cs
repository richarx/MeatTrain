using System.Collections;
using Final_Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace End_Scene
{
    public class EndScene : MonoBehaviour
    {
        [SerializeField] private int levelToTriggerEndScene;
        [SerializeField] private float duration;
        [SerializeField] private Transform trainTail;
        [SerializeField] private AudioClip finalCrunch;
        [SerializeField] private Image blackScreen;

        public static UnityEvent OnTriggerEndScene = new UnityEvent();
        public static UnityEvent OnTriggerEndSceneBlackScreen = new UnityEvent();
        
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

            isTriggered = true;
            
            OnTriggerEndScene?.Invoke();
            StartCoroutine(EndSceneCoroutine());
        }

        private IEnumerator EndSceneCoroutine()
        {
            ShakeEverything();
            float timer = 0.0f;
            while (timer <= duration)
            {
                MoveCameraTowardsTarget(timer);
                ZoomCamera(timer);
                MoveTrainTailTowardsTarget(timer);
                SlowDownTime(timer);
                yield return null;
                timer += Time.deltaTime;
            }

            Time.timeScale = 1.0f;
            ActivateBlackScreen();
            PlaySpookySound();
            yield return new WaitForSeconds(6.5f);
            TriggerFinalAnimation();
        }

        private Vector3 moveCameraVelocity;
        private void MoveCameraTowardsTarget(float timer)
        {
            if (timer <= 3.0f)
                return;
            
            Vector3 current = mainCamera.transform.position;
            Vector3 target = new Vector3(2.16f, 0.83f, -10.0f);

            current = Vector3.SmoothDamp(current, target, ref moveCameraVelocity, duration / 3.0f);
            mainCamera.transform.position = current;
        }

        private float zoomVelocity;
        private void ZoomCamera(float timer)
        {
            if (timer <= 3.0f)
                return;
            
            float current = mainCamera.orthographicSize;
            float target = 1.0f;

            current = Mathf.SmoothDamp(current, target, ref zoomVelocity, duration / 2.0f);
            mainCamera.orthographicSize = current;
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
            if (timer >= 10.0f)
                Time.timeScale = 1.0f - Tools.Tools.NormalizeValue(timer, 0.0f, duration * 2.0f);
        }

        private void ShakeEverything()
        {
            StartCoroutine(Tools.Tools.Shake(mainCamera.transform, duration, 0.01f));
            StartCoroutine(Tools.Tools.Shake(trainTail, duration, 0.01f));
        }

        private void ActivateBlackScreen()
        {
            OnTriggerEndSceneBlackScreen?.Invoke();
            blackScreen.gameObject.SetActive(true);
        }

        private void PlaySpookySound()
        {
            SFXManager.Instance.PlaySFX(finalCrunch, 0.5f);
        }

        private void TriggerFinalAnimation()
        {
            FinalAnimation.instance.TriggerFinalAnimation();
        }
    }
}
