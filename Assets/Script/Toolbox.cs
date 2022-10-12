using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Toolbox : MonoBehaviour
{
    private static Toolbox _instance;
    public StatsManager StatsManager;

    public static Toolbox GetInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject("Toolbox");
            _instance = go.AddComponent<Toolbox>();
            DontDestroyOnLoad(go);
        }
        return _instance;
    }

    private void Awake() 
    {
        if (StatsManager == null)
        {
            StatsManager = FindObjectOfType<StatsManager>();

            if (StatsManager == null)
            {
            GameObject go = new GameObject("StatsManager");
            StatsManager = go.AddComponent<StatsManager>();
            DontDestroyOnLoad(go);
            }
        }
    }
}
