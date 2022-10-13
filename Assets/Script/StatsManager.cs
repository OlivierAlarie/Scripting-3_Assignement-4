using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [Header("Time Variables")]
    public float Timer;
    public int TimeApprox;
    public bool IsPlaying = false;
    [SerializeField] private TextMeshProUGUI _timerTMP;

    [Header("HUD Variables")]
    public int RetryCounter;
    [SerializeField] private TextMeshProUGUI _retryCountTMP;
    public int RoundCounter;
    [SerializeField] private TextMeshProUGUI _roundCountTMP;

    [Header("Turn Stage Variables")]
    public string TurnStage;
    [SerializeField] private TextMeshProUGUI _turnStageTMP;

    [Header("Singleton Variable")]
    public static StatsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (IsPlaying)
        {
            TimeCounter();
        }
    }

    public void TimeCounter()
    {
        Timer += Time.deltaTime;
        TimeApprox = (int)Timer;
        SetTimerText();
    }

    public void SetPlaying(bool value)
    {
        IsPlaying = value;
    }

    public void SetTimerText()
    {
        _timerTMP.text = ("Timer: " + TimeApprox.ToString());
    }

    public void SetStageText(string stage)
    {
        _turnStageTMP.text = ("Turn: " + stage);
    }

    public void IncreaseRetryText()
    {
        RetryCounter++;
        _retryCountTMP.text = ("Retry: " + RetryCounter);
    }

    public void IncreaseRoundText()
    {
        RoundCounter++;
        _roundCountTMP.text = ("Round: " + RoundCounter);
    }

    public void ResetRoundText()
    {
        RoundCounter = 0;
        _roundCountTMP.text = ("Round: " + RoundCounter);
    }

    public void ResetGameText()
    {
        RoundCounter = 0;
        _roundCountTMP.text = ("Round: " + RoundCounter);
        RetryCounter = 0;
        _retryCountTMP.text = ("Retry: " + RetryCounter);
        Timer = 0f;
        TimeApprox = 0;
        _timerTMP.text = ("Timer: " + TimeApprox.ToString());
    }
}
