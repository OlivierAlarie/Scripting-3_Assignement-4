using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void StartButton() 
    {
        SceneManager.LoadScene(1);
    }

    public void QuitButton() 
    {
        Application.Quit();
    }
}
