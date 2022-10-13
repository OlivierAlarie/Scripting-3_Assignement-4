using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Toolbox : MonoBehaviour
{
    private static Toolbox _instance;

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
/*
    private StatsManager _statsManager;

    public StatsManager GetStatsManager()
    {
        return _statsManager;
    }

    private void Awake() 
    {
        if (_instance != null)
        {
            Debug.LogError("Trying to instantiate a second instance of Toolbox");
            Destroy(gameObject);
        }        
        else
        {
            if (_statsManager == null)
            {
                _statsManager = FindObjectOfType<StatsManager>();
                if (_statsManager == null)
                {
                    //GameObject go = new GameObject("StatsManager");
                    GameObject go = Instantiate(Resources.Load("Prefabs/UI") as GameObject);
                    go.transform.parent = transform;
                    _statsManager = go.AddComponent<StatsManager>();
                    //DontDestroyOnLoad(go);
                }
            }           
        }
    }*/
}
