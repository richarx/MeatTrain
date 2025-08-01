using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelHandler : MonoBehaviour
{
    public static LevelHandler Instance;

    public static UnityEvent<float> OnLevelChange = new UnityEvent<float>();

    private float currentLevel;
    [HideInInspector] public float CurrentLevel => currentLevel;

    void Start()
    {
        Instance = this;  
    }

    public void ChangeLevel()
    {
        currentLevel += 1f;
        OnLevelChange.Invoke(currentLevel);
    }
}
