using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, BATTLEPHASE, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState State;
    public Text DialogueText;
    public HandManager HandManager;
    public Sprite[] HandsSprites;

    private float _playerDamagedTimeDelay = 1f;
    private float _playerDamagedFlashDelay = 0.1f;
    private bool _playerIsDamaged = false;

    [Header("Player Variables")]
    public GameObject PlayerPrefab;
    private Unit _playerUnit;
    public BattleHUD PlayerHUD;
    public Transform PlayerBattleStation;
    [SerializeField] private SpriteRenderer _playerBaseSprtRndrr;
    [SerializeField] private Image _playerHandImg;

    [SerializeField] private GameObject _playerButtons;

    [Header("Enemy Variables")]
    public GameObject EnemyPrefab;
    private Unit _enemyUnit;
    public BattleHUD EnemyHUD;
    public Transform EnemyBattleStation;
    [SerializeField] private SpriteRenderer _enemyBaseSprtRndrr;
    [SerializeField] private Image _enemyHandImg;

    public string[] Choices;

    #region Set Up

    void Start()
    {
        HandManager = GetComponentInChildren<HandManager>(true);

        ChangeTurnStage(BattleState.START);

        StartCoroutine(SetupBattle());
        //Toolbox.GetInstance().StatsManager();
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(PlayerPrefab, PlayerBattleStation);
        _playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(EnemyPrefab, EnemyBattleStation);
        _enemyUnit = enemyGO.GetComponent<Unit>();

        DialogueText.text = _enemyUnit.UnitName + " is ready!";

        PlayerHUD.SetHUD(_playerUnit);
        EnemyHUD.SetHUD(_enemyUnit);
        yield return new WaitForSeconds(2f);

        _playerButtons.SetActive(true);

        ChangeTurnStage(BattleState.PLAYERTURN);

        PlayerTurn();
    }

    #endregion

    #region Turns

    void PlayerTurn()
    {
        DialogueText.text = "Choose an action:";
        _playerUnit.CurrentHand = "Unknown";
        _enemyUnit.CurrentHand = "Unknown";

        _playerHandImg.sprite = HandsSprites[0];
        _enemyHandImg.sprite = HandsSprites[0];

        StatsManager.Instance.IncreaseRoundText();
    }

    IEnumerator EnemyTurn()
    {
        string enemyChoice = Choices[Random.Range(0, Choices.Length)];

        if (enemyChoice == "Rock")
        {
            _enemyUnit.CurrentHand = "Rock";
            HandManager.PlayHand("Rock");

            _enemyHandImg.sprite = HandsSprites[1];
        }

        else if (enemyChoice == "Paper")
        {
            _enemyUnit.CurrentHand = "Paper";
            HandManager.PlayHand("Paper");

            _enemyHandImg.sprite = HandsSprites[2];
        }

        else if (enemyChoice == "Scissors")
        {
            _enemyUnit.CurrentHand = "Scissors";
            HandManager.PlayHand("Scissors");

            _enemyHandImg.sprite = HandsSprites[3];
        }

        yield return new WaitForSeconds(1f);

        ChangeTurnStage(BattleState.BATTLEPHASE);

        StartCoroutine(ResultTurn());
    }

    IEnumerator ResultTurn()
    {
        yield return new WaitForSeconds(1f);

        if (_playerUnit.CurrentHand == "Rock")
        {
            if (_enemyUnit.CurrentHand == "Scissors")
            {
                StartCoroutine(PlayerAttack());
            }
            else if (_enemyUnit.CurrentHand == "Paper")
            {
                StartCoroutine(EnemyAttack());
            }
            else
            {
                StartCoroutine(Tiebreaker());
            }
        }
        else if (_playerUnit.CurrentHand == "Paper")
        {
            if (_enemyUnit.CurrentHand == "Rock")
            {
                StartCoroutine(PlayerAttack());

            }
            else if (_enemyUnit.CurrentHand == "Scissors")
            {
                StartCoroutine(EnemyAttack());
            }
            else
            {
                StartCoroutine(Tiebreaker());
            }
        }
        else if (_playerUnit.CurrentHand == "Scissors")
        {
            if (_enemyUnit.CurrentHand == "Paper")
            {
                StartCoroutine(PlayerAttack());
            }
            else if (_enemyUnit.CurrentHand == "Rock")
            {
                StartCoroutine(EnemyAttack());
            }
            else
            {
                StartCoroutine(Tiebreaker());
            }
        }
    }

    void EndBattle()
    {
        StatsManager.Instance.SetPlaying(false);
        if (State == BattleState.WON)
        {
            DialogueText.text = "You won the battle!";
            SceneManager.LoadScene("End");
        }
        else if (State == BattleState.LOST)
        {
            DialogueText.text = "You were defeated.";
            SceneManager.LoadScene("BadEnd");
        }
    }

    #endregion

    #region Player Choice

    public void RockButton()
    {
        if (State != BattleState.PLAYERTURN && _playerUnit.CurrentHand != "Unknown")
        {
            return;
        }

        HandManager.PlayHand("Rock");
        DialogueText.text = "You have chosen rock";
        _playerUnit.CurrentHand = "Rock";

        _playerHandImg.sprite = HandsSprites[1];

        StartCoroutine(PlayerHand());
    }

    public void PaperButton()
    {
        if (State != BattleState.PLAYERTURN && _playerUnit.CurrentHand != "Unknown")
        {
            return;
        }

        HandManager.PlayHand("Paper");
        DialogueText.text = "You have chosen paper";
        _playerUnit.CurrentHand = "Paper";

        _playerHandImg.sprite = HandsSprites[2];

        StartCoroutine(PlayerHand());
    }

    public void ScissorsButton()
    {
        if (State != BattleState.PLAYERTURN && _playerUnit.CurrentHand != "Unknown")
        {
            return;
        }

        HandManager.PlayHand("Scissors");
        DialogueText.text = "You have chosen scissors";
        _playerUnit.CurrentHand = "Scissors";

        _playerHandImg.sprite = HandsSprites[3];

        StartCoroutine(PlayerHand());
    }

    IEnumerator PlayerHand()
    {
        ChangeTurnStage(BattleState.ENEMYTURN);
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemyTurn());
    }

    #endregion

    #region Attacks

    IEnumerator PlayerAttack()
    {
        bool isDead = _enemyUnit.TakeDamage(_playerUnit.Damage);

        yield return new WaitForSeconds(1f);

        EnemyHUD.SetHP(_enemyUnit.CurrentHP);
        DialogueText.text = _playerUnit.UnitName + " has won this round!";

        _playerIsDamaged = true;
        StartCoroutine(PlayerDamagedFlash(_enemyBaseSprtRndrr));
        StartCoroutine(HandlePlayerDamagedDelay());

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            ChangeTurnStage(BattleState.WON);
            EndBattle();
        }
        else
        {
            ChangeTurnStage(BattleState.PLAYERTURN);
            PlayerTurn();
        }
    }

    IEnumerator EnemyAttack()
    {
        DialogueText.text = _enemyUnit.UnitName + " has won this round...";

        yield return new WaitForSeconds(1f);

        bool isDead = _playerUnit.TakeDamage(_enemyUnit.Damage);

        PlayerHUD.SetHP(_playerUnit.CurrentHP);

        _playerIsDamaged = true;
        StartCoroutine(PlayerDamagedFlash(_playerBaseSprtRndrr));
        StartCoroutine(HandlePlayerDamagedDelay());

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            ChangeTurnStage(BattleState.LOST);
            EndBattle();
        }
        else
        {
            ChangeTurnStage(BattleState.PLAYERTURN);
            PlayerTurn();
        }
    }

    IEnumerator Tiebreaker()
    {
        DialogueText.text = "Tiebreaker! Let's continue!";

        yield return new WaitForSeconds(1f);
        ChangeTurnStage(BattleState.PLAYERTURN);
        PlayerTurn();
    }

    #endregion

    #region Display Utility

    public IEnumerator PlayerDamagedFlash(SpriteRenderer baseSprite)
    {
        while (_playerIsDamaged)
        {
            baseSprite.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(_playerDamagedFlashDelay);
            baseSprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(_playerDamagedFlashDelay);
        }
    }

    public IEnumerator HandlePlayerDamagedDelay()
    {
        yield return new WaitForSeconds(_playerDamagedTimeDelay);
        _playerIsDamaged = false;
    }

    public void ChangeTurnStage(BattleState newState)
    {
        State = newState;
        StatsManager.Instance.SetStageText(State.ToString());
    }

    #endregion
}
