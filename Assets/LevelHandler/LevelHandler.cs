using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelHandler : MonoBehaviour
{
    private LevelHandler Instance;

    public static UnityEvent OnLevelChange = new UnityEvent();

    void Start()
    {
        Instance = this;  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeLevel()
    {
        OnLevelChange.Invoke();
    }
}
