using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Stats Manager")]
    [SerializeField] private StatsManager _statsManager;

    private void Start() {
        //_statsManager = GetComponent.
    }
    
    private void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
        _statsManager.IsPlaying = false;
    }

    private void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _statsManager.IsPlaying = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
