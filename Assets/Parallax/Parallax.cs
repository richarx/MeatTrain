using System.Collections;
using MiniMap;
using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Parallax
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private Transform parallax1;
        [SerializeField] private Transform parallax2;
        [SerializeField] private float speed;
        [SerializeField] private Vector2 direction;
        [SerializeField] private Map.Biome biome;
        [SerializeField] private bool isScaling;

        [HideInInspector] public UnityEvent OnReachExit = new UnityEvent();

        private float initialDistance;
        private Vector3 exitPosition;
        private Vector3 firstPosition;
        private Vector3 secondPosition;
        private Vector3 thirdPosition;

        private bool isMoving = true;
        private bool isStopRequested;
        private float globalSpeed = 0.0f;

        private Vector3 velocity;

        public Map.Biome currentBiome => biome;

        private void Start()
        {
            firstPosition = parallax1.position;
            initialDistance = Vector3.Distance(parallax1.position, parallax2.position);
            ComputePositions(initialDistance);

            if (currentBiome == Map.Biome.Forest)
                Debug.Log($"first : {firstPosition} / second : {secondPosition} / third : {thirdPosition}");
            
            LevelHandler.LevelHandler.OnLevelChange.AddListener((level) => StartCoroutine(UpdateScale(level)));
        }

        private void ComputePositions(float distance)
        {
            Vector3 delta = (direction * -1.0f).ToVector3().normalized * distance;
            secondPosition = firstPosition + delta;
            thirdPosition = secondPosition + delta;
            exitPosition = firstPosition - delta;
        }

        private IEnumerator UpdateScale(int level)
        {
            if (!isScaling)
                yield break;
            
            float duration = LevelHandler.LevelHandler.Instance.LevelUpAnimationDuration;

            Vector3 currentScale = parallax1.localScale;
            float scalePerLevel = 1.0f - (0.05f * (level - 1));
            Vector3 targetScale = Vector3.one * scalePerLevel;
            
            ComputePositions(initialDistance * scalePerLevel);

            while (currentScale.magnitude > targetScale.magnitude)
            {
                currentScale = Vector3.SmoothDamp(currentScale, targetScale, ref velocity, duration);
                parallax1.localScale = currentScale;
                parallax2.localScale = currentScale;
                
                yield return null;
            }
        }

        private void Update()
        {
            if (!isMoving)
                return;

            float moveSpeed = speed * globalSpeed;
            
            parallax1.position += direction.ToVector3().normalized * (moveSpeed * Time.deltaTime);
            parallax2.position += direction.ToVector3().normalized * (moveSpeed * Time.deltaTime);

            if (HasReachedExit(parallax1.position))
                ResetParallax(parallax1);

            if (HasReachedExit(parallax2.position))
                ResetParallax(parallax2);
        }

        private void ResetParallax(Transform parallax)
        {
            parallax.position = secondPosition;

            if (isStopRequested)
                parallax.gameObject.SetActive(false);
            
            OnReachExit?.Invoke();
        }

        private bool HasReachedExit(Vector3 position)
        {
            return isMoving && globalSpeed > 0.0f && thirdPosition.Distance(position) >= thirdPosition.Distance(exitPosition);
        }

        public void SetSpeed(float newGlobalSpeed)
        {
            globalSpeed = newGlobalSpeed;
        }

        public void SetState(bool state, bool isInstant)
        {
            if (state || isInstant)
            {
                parallax1.gameObject.SetActive(state);
                parallax2.gameObject.SetActive(state);
            }
            
            isStopRequested = !state;

            if (state && !isInstant)
            {
                parallax1.position = secondPosition;
                parallax2.position = thirdPosition;
            }
        }
    }
}
