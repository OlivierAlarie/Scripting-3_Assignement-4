using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool _isPlaying;
    private float _timer;
    private float _totalTimer;
    private int _score;
    private int _totalScore;
    private int _totalRetries;

    private void Start()
    {
        StartLevel();
    }

    private void Update()
    {
        if (_isPlaying)
        {
            _timer += Time.deltaTime;
            Toolbox.GetInstance().GetUIManager().SetTimerText((int)_timer);
        }
    }

    public void AddScore(int value)
    {
        _score += value;
        Toolbox.GetInstance().GetUIManager().SetScoreText(_score);
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
        _isPlaying = false;
    }

    private void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
        _isPlaying = true;
    }

    public void StartLevel()
    {
        _score = 0;
        Toolbox.GetInstance().GetUIManager().SetScoreText(_score);
        _timer = 0;
        Toolbox.GetInstance().GetUIManager().SetTimerText((int)_timer);

        Toolbox.GetInstance().GetUIManager().ShowHud(true);
        Resume();
    }

    public void PauseLevel()
    {
        Toolbox.GetInstance().GetUIManager().ShowPauseOverlay(true);
        Pause();
    }

    public void ResumeLevel()
    {
        Toolbox.GetInstance().GetUIManager().ShowPauseOverlay(false);
        Resume();
    }

    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        if(SceneManager.GetSceneByBuildIndex(nextSceneIndex).name != "End")
        {
            StartLevel();
        }
        else
        {
            EndGame();
        }
    }

    public void LoseLevel()
    {
        Toolbox.GetInstance().GetUIManager().ShowRetryOverlay(true);
        Toolbox.GetInstance().GetUIManager().ShowHud(false);
        Pause();
    }

    public void Retry()
    {
        Toolbox.GetInstance().GetUIManager().ShowRetryOverlay(false);

        _totalRetries++;
        StartLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        Toolbox.GetInstance().GetUIManager().ShowFinishOverlay(true);

        //Toolbox.GetInstance().GetUIManager().SetTotalScoreText(_totalScore);
        //Toolbox.GetInstance().GetUIManager().SetTotalTimerText((int)_totalTimer);
        Toolbox.GetInstance().GetUIManager().SetTimerText((int)_totalTimer);
        Toolbox.GetInstance().GetUIManager().SetTotalRetriesText(_totalRetries);
    }

    public void QuitLevel()
    {
        Toolbox.GetInstance().Unload();

        SceneManager.LoadScene(0);
    }
}
