using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniMap
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] private Transform display;
        [SerializeField] private Transform parentHolder;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private int levelRequiredToUnlock;
        [SerializeField] private float distanceFromPlanetCenter;
        [SerializeField] private float startingAngleBetweenNodes;
        [SerializeField] private float angleBetweenNodesScaling;
        [SerializeField] private float sizeScaling;
        
        private List<Transform> nodes = new List<Transform>();
        private Transform tailNode;

        private int length = 3;

        private float currentAngle = 0.0f;
        private float angleBetweenNodes = 0.0f;

        private bool isUnlocked;
        
        private void Start()
        {
            LevelHandler.LevelHandler.OnLevelChange.AddListener((level) =>
            {
                if (!isUnlocked && level >= levelRequiredToUnlock)
                    StartCoroutine(Unlock());
                
                IncreaseTrainSize(level);
            });

            SetupInitialNodes();
        }

        private IEnumerator Unlock()
        {
            isUnlocked = true;
            yield return Tools.Tools.TweenPosition(display, display.position.x, display.position.y - display.localPosition.y, 0.3f);
        }

        private void SetupInitialNodes()
        {
            angleBetweenNodes = startingAngleBetweenNodes;
            for (int i = 0; i < parentHolder.childCount - 1; i++)
            {
                nodes.Add(parentHolder.GetChild(i));
            }

            tailNode = parentHolder.GetChild(parentHolder.childCount - 1);
        }

        private void IncreaseTrainSize(int level)
        {
            length += 1;
            angleBetweenNodes = startingAngleBetweenNodes + (angleBetweenNodesScaling * level);
        }

        private void Update()
        {
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
                length = Mathf.Max(1, length - 1);
            
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
                length = Mathf.Min(100, length + 1);

            UpdateTrainPosition();
            UpdateTrainLength();
            UpdateNodesPosition();
        }

        private void UpdateTrainPosition()
        {
            float distanceOnPlanet = Map.instance.ComputePositionOnPlanet();
            currentAngle = 360.0f - Tools.Tools.NormalizeValueInRange(distanceOnPlanet, 0.0f, Map.instance.WorldSize, 0.0f, 360.0f);
        }

        private void UpdateNodesPosition()
        {
            Vector2 center = parentHolder.position;
            int currentLevel = LevelHandler.LevelHandler.Instance.CurrentLevel;
            
            float angle = currentAngle;
            for (int i = 0; i < nodes.Count; i++)
            {
                Vector2 direction = Vector2.right.AddAngleToDirection(angle).normalized;
                Vector3 position = center + direction * distanceFromPlanetCenter;
                nodes[i].position = position;
                nodes[i].localRotation = direction.AddAngleToDirection(-90.0f).ToRotation();
                nodes[i].localScale = Vector3.one * (0.07f + sizeScaling * currentLevel);
                angle += angleBetweenNodes;
            }
            
            Vector2 tailDirection = Vector2.right.AddAngleToDirection(angle).normalized;
            Vector3 tailPosition = center + tailDirection * distanceFromPlanetCenter;
            tailNode.position = tailPosition;
            tailNode.localRotation = tailDirection.AddAngleToDirection(-90.0f).ToRotation();
            tailNode.localScale = Vector3.one * (0.07f + sizeScaling * currentLevel);
        }
        
        

        private void UpdateTrainLength()
        {
            int currentDelta = nodes.Count - length;
            
            if (currentDelta > 0)
            {
                for (int i = nodes.Count - 1; i > 0 && currentDelta > 0; i--)
                {
                    Destroy(nodes[i].gameObject);
                    nodes.RemoveAt(i);
                    currentDelta -= 1;
                }
            }
            else if (currentDelta < 0)
            {
                for (int i = 0; i < Mathf.Abs(currentDelta); i++)
                    nodes.Add(Instantiate(nodePrefab, parentHolder).transform);
            }
        }
    }
}
