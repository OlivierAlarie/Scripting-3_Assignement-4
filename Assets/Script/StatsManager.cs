using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsManager : MonoBehaviour
{
    [Header("Time Variables")]
    public float Timer;
    public float TimeApprox;
    public bool IsPlaying = false;
    [SerializeField] private TextMeshProUGUI _timerTMP;

    [Header("HUD Variables")]
    public int RetryCounter;
    [SerializeField] private TextMeshProUGUI _retryCountTMP;

    [Header("Turn Stage Variables")]
    public string TurnStage;
    [SerializeField] private TextMeshProUGUI _turnStageTMP;

    [Header("Singleton Variable")]
    public static StatsManager Instance;

    private void Awake()
    {
        if(Instance == null)
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
        Scene currentScene = SceneManager.GetActiveScene();

        string sceneName = currentScene.name;

        if (sceneName == "Combat")
        {
            IsPlaying = true;
            StatsUpdate();
        }
        else if (sceneName == "End")
        {
            IsPlaying = false;
            StatsUpdate();
        }
        else
        {
            IsPlaying = false;
            _retryCountTMP.text = ("");
            _turnStageTMP.text = ("");
            _timerTMP.text = ("");
        }
        
        TimeCounter();
    }
    
    public void TimeCounter()
    {
        if (IsPlaying)
        {
            Timer += Time.deltaTime;
            TimeApprox = (int)Timer;
        }
    }

    public void StatsUpdate()
    {
        _retryCountTMP.text = ("Retry: " + RetryCounter.ToString());
        _turnStageTMP.text = ("Turn: " + BattleState.ENEMYTURN.ToString());
        _timerTMP.text = ("Timer: " + TimeApprox.ToString());
    }
}
