using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenManager : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ReturnButton() 
    {
        //StatsManager.Instance.IncreaseRetryText();
        StatsManager.Instance.ResetGameText();
        SceneManager.LoadScene(0);
        //StatsManager.Instance.Timer = 0f;
        //StatsManager.Instance.RetryCounter = 0;
    }

        public void RetryButton() 
    {
        StatsManager.Instance.IncreaseRetryText();
        SceneManager.LoadScene(0);
        //StatsManager.Instance.Timer = 0f;
        //StatsManager.Instance.RetryCounter = 0;
    }
}
