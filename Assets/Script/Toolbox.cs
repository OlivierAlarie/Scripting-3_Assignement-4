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

    public void Unload()
    {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    //Level Manager
    private LevelManager _levelManager;
    public LevelManager GetLevelManager()
    {
        return _levelManager;
    }

    //UI Manager
    private UIManager _UIManager;
    public UIManager GetUIManager()
    {
        return _UIManager;
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
            if (_levelManager == null)
            {

                _levelManager = FindObjectOfType<LevelManager>();
                if (_levelManager == null)
                {
                    GameObject go = new GameObject("LevelManager");
                    go.transform.parent = transform;
                    _levelManager = go.AddComponent<LevelManager>();
                }
            }

            if (_UIManager == null)
            {
                _UIManager = FindObjectOfType<UIManager>();
                if (_UIManager == null)
                {
                    GameObject go = Instantiate(Resources.Load("Prefabs/UI") as GameObject);
                    go.transform.parent = transform;
                    _UIManager = go.GetComponent<UIManager>();
                }
            }

        }
    }
}
