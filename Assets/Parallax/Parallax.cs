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

        public UnityEvent OnReachExit = new UnityEvent();

        private SpriteRenderer parallax1Sr;
        private SpriteRenderer parallax2Sr;
        
        private Vector3 exitPosition;
        private Vector3 firstPosition;
        private Vector3 secondPosition;

        private bool isMoving = true;
        private float globalSpeed = 0.0f;

        private Sprite nextBiome;
        private int biomeSpritesToUpdateCount;
        
        private void Start()
        {
            firstPosition = parallax1.position;
            secondPosition = parallax2.position;
            exitPosition = firstPosition + (firstPosition - secondPosition);

            parallax1Sr = parallax1.GetComponent<SpriteRenderer>();
            parallax2Sr = parallax2.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (!isMoving)
                return;

            float moveSpeed = speed * globalSpeed;
            
            parallax1.position += direction.ToVector3().normalized * (moveSpeed * Time.deltaTime);
            parallax2.position += direction.ToVector3().normalized * (moveSpeed * Time.deltaTime);

            if (HasReachedExit(parallax1.position))
                ResetParallax(parallax1, parallax1Sr);

            if (HasReachedExit(parallax2.position))
                ResetParallax(parallax2, parallax2Sr);
        }

        private void ResetParallax(Transform parallax, SpriteRenderer spriteRenderer, bool triggerEvent = true)
        {
            parallax.position = secondPosition;

            if (biomeSpritesToUpdateCount > 0)
            {
                spriteRenderer.sprite = nextBiome;
                biomeSpritesToUpdateCount -= 1;
            }
            
            if (triggerEvent)
                OnReachExit?.Invoke();
        }

        public void SetNewSprite(Sprite newSprite, bool isInstant = false)
        {
            nextBiome = newSprite;
            biomeSpritesToUpdateCount = 2;

            if (isInstant)
            {
                bool first = parallax1.position.x > parallax2.position.x;
                ResetParallax(first ? parallax1 : parallax2, first ? parallax1Sr : parallax2Sr, false);
            }
        }

        private bool HasReachedExit(Vector3 position)
        {
            return isMoving && globalSpeed > 0.0f && firstPosition.Distance(position) >= firstPosition.Distance(exitPosition);
        }

        public void StartMoving()
        {
            isMoving = true;
        }

        public void StopMoving()
        {
            isMoving = false;
        }

        public void SetSpeed(float newGlobalSpeed)
        {
            globalSpeed = newGlobalSpeed;
        }
    }
}
