using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    
    
    public void Pause()
    {
        gameObject.SetActive(true);
        //Cursor.lockState = CursorLockMode.Confined;
       //Cursor.visible = true;
        Time.timeScale = 0;
        //_statsManager.IsPlaying = false;
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
        Time.timeScale = 1f;
        //_statsManager.IsPlaying = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
    }
}
