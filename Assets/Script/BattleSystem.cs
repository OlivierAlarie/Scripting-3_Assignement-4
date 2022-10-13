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

    [Header("Player Variables")]
    public GameObject playerPrefab;
    private Unit _playerUnit;
    public BattleHUD playerHUD;
    public Transform playerBattleStation;
    [SerializeField] private SpriteRenderer _playerBaseSprtRndrr;
    [SerializeField] private Image _playerHandImg;

    [SerializeField] private GameObject _PlayerButtons;
    private float _playerDamagedTimeDelay = 1f;
    private float _playerDamagedFlashDelay = 0.1f;
    private bool _playerIsDamaged = false;

    [Header("Enemy Variables")]
    public GameObject enemyPrefab;
    Unit enemyUnit;
    public BattleHUD enemyHUD;
    public Transform enemyBattleStation;
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
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        _playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        DialogueText.text = enemyUnit.UnitName + " is ready!";

        playerHUD.SetHUD(_playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        yield return new WaitForSeconds(2f);

        _PlayerButtons.SetActive(true);

        ChangeTurnStage(BattleState.PLAYERTURN);

        PlayerTurn();
    }

    #endregion

    #region Turns

    void PlayerTurn()
    {
        DialogueText.text = "Choose an action:";
        _playerUnit.CurrentHand = "Unknown";
        enemyUnit.CurrentHand = "Unknown";

        _playerHandImg.sprite = HandsSprites[0];
        _enemyHandImg.sprite = HandsSprites[0];

        StatsManager.Instance.IncreaseRoundText();
    }

    IEnumerator EnemyTurn()
    {
        string enemyChoice = Choices[Random.Range(0, Choices.Length)];

        if (enemyChoice == "Rock")
        {
            enemyUnit.CurrentHand = "Rock";
            HandManager.PlayHand("Rock");

            _enemyHandImg.sprite = HandsSprites[1];
        }

        else if (enemyChoice == "Paper")
        {
            enemyUnit.CurrentHand = "Paper";
            HandManager.PlayHand("Paper");

            _enemyHandImg.sprite = HandsSprites[2];
        }

        else if (enemyChoice == "Scissors")
        {
            enemyUnit.CurrentHand = "Scissors";
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
            if (enemyUnit.CurrentHand == "Scissors")
            {
                StartCoroutine(PlayerAttack());
            }
            else if (enemyUnit.CurrentHand == "Paper")
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
            if (enemyUnit.CurrentHand == "Rock")
            {
                StartCoroutine(PlayerAttack());

            }
            else if (enemyUnit.CurrentHand == "Scissors")
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
            if (enemyUnit.CurrentHand == "Paper")
            {
                StartCoroutine(PlayerAttack());
            }
            else if (enemyUnit.CurrentHand == "Rock")
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
        bool isDead = enemyUnit.TakeDamage(_playerUnit.Damage);

        yield return new WaitForSeconds(1f);

        enemyHUD.SetHP(enemyUnit.CurrentHP);
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
        DialogueText.text = enemyUnit.UnitName + " has won this round...";

        yield return new WaitForSeconds(1f);

        bool isDead = _playerUnit.TakeDamage(enemyUnit.Damage);

        playerHUD.SetHP(_playerUnit.CurrentHP);

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

    public IEnumerator PlayerDamagedFlash(SpriteRenderer baseSprite) //IEnumerator is useful to add delays
    {
        while (_playerIsDamaged)
        {
            baseSprite.color = new Color(1f, 1f, 1f, 0f); //Flashing animation from max opacity to none (r,g,b,a)
            yield return new WaitForSeconds(_playerDamagedFlashDelay);
            baseSprite.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(_playerDamagedFlashDelay);
        }
    }

    public IEnumerator HandlePlayerDamagedDelay() //Timer on the player damaged state
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
