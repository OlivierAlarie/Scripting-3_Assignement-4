using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _finishOverlay;
    [SerializeField] private GameObject _retryOverlay;
    [SerializeField] private GameObject _pauseOverlay;
    [SerializeField] private GameObject _hud;

    [SerializeField] private TextMeshProUGUI timerTMP;
    //[SerializeField] private TextMeshProUGUI scoreTMP;

    //[SerializeField] private TextMeshProUGUI totalTimerTMP;
    //[SerializeField] private TextMeshProUGUI totalScoreTMP;
    [SerializeField] private TextMeshProUGUI totalRetriesTMP;

    public void NextLevelButton()
    {
        //Toolbox.GetInstance().GetLevelManager().NextLevel();
    }

    //RetryOverlay
    public void ShowRetryOverlay(bool value)
    {
        _retryOverlay.SetActive(value);
    }
    public void RetryButton()
    {
        Toolbox.GetInstance().GetLevelManager().Retry();
    }

    //PauseOverlay
    public void ShowPauseOverlay(bool value)
    {
        _pauseOverlay.SetActive(value);
    }
    public void ContinueButton()
    {
        Toolbox.GetInstance().GetLevelManager().ResumeLevel();
    }

    //FinishOverlay
    public void ShowFinishOverlay(bool value)
    {
        _finishOverlay.SetActive(value);
    }
/*
    public void SetTotalScoreText(int value)
    {
        totalScoreTMP.text = "Total Score : " + value.ToString();
    }*/
/*
    public void SetTotalTimerText(int value)
    {
        totalTimerTMP.text = "Total Timer : " + value.ToString() + " seconds";
    }
*/
    public void SetTotalRetriesText(int value)
    {
        totalRetriesTMP.text = "Total Retries : " + value.ToString();
    }


    public void ReturnToMainMenuButton()
    {
        Toolbox.GetInstance().GetLevelManager().QuitLevel();
    }

    //HUD
    public void ShowHud(bool value)
    {
        _hud.SetActive(value);
    }

    public void SetScoreText(int value)
    {
        //scoreTMP.text = "Score : "+value.ToString();
    }

    public void SetTimerText(int value)
    {
        timerTMP.text = "Timer : " + value.ToString() + " seconds";
    }
}
