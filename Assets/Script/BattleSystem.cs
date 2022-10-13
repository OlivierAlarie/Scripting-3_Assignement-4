using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, BATTLEPHASE, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public Text dialogueText;
    public HandManager HandManager;

    [Header("Player Variables")]
    public GameObject playerPrefab;
    private Unit _playerUnit;
    public Transform playerBattleStation;
    public BattleHUD playerHUD;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;

    [SerializeField] private GameObject _PlayerButtons;
    private float _playerDamagedTimeDelay = 1f;
    private float _playerDamagedFlashDelay = 0.1f;
    private bool _playerIsDamaged = false;
    
    [SerializeField] private GameObject _playerRockSprite;
    [SerializeField] private GameObject _playerPaperSprite;
    [SerializeField] private GameObject _playerScissorsSprite;
    [SerializeField] private GameObject _playerUnknownSprite;

    [Header("Enemy Variables")]
    public GameObject enemyPrefab;
    Unit enemyUnit;
    public Transform enemyBattleStation;
    public BattleHUD enemyHUD;
    [SerializeField] private SpriteRenderer _enemySpriteRenderer;

    public string[] Choices;
    
    [SerializeField] private GameObject _enemyRockSprite;
    [SerializeField] private GameObject _enemyPaperSprite;
    [SerializeField] private GameObject _enemyScissorsSprite;
    [SerializeField] private GameObject _enemyUnknownSprite;

    #region Set Up

    void Start()
    {
        HandManager = GetComponentInChildren<HandManager>(true);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        //Toolbox.GetInstance().StatsManager();
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        _playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.UnitName + " is ready!";

        playerHUD.SetHUD(_playerUnit);
        enemyHUD.SetHUD(enemyUnit);
        yield return new WaitForSeconds(2f);

        _PlayerButtons.SetActive(true);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    #endregion

    #region Attacks

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(_playerUnit.Damage);

        yield return new WaitForSeconds(1f);

        enemyHUD.SetHP(enemyUnit.CurrentHP);
        dialogueText.text = _playerUnit.UnitName + " has won this round!";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyAttack()
    {
        dialogueText.text = enemyUnit.UnitName + " has won this round...";

        yield return new WaitForSeconds(1f);

        bool isDead = _playerUnit.TakeDamage(enemyUnit.Damage);

        playerHUD.SetHP(_playerUnit.CurrentHP);

        _playerIsDamaged = true;
        StartCoroutine(PlayerDamagedFlash());
        StartCoroutine(HandlePlayerDamagedDelay());

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator Tiebreaker()
    {
        dialogueText.text = "Tiebreaker! Let's continue!";

        yield return new WaitForSeconds(1f);
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    #endregion

    #region Turns

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
        _playerUnit.CurrentHand = "Unknown";
        enemyUnit.CurrentHand = "Unknown";
        _playerRockSprite.SetActive(false);
        _playerPaperSprite.SetActive(false);
        _playerScissorsSprite.SetActive(false);
        _playerUnknownSprite.SetActive(true);
        _enemyRockSprite.SetActive(false);
        _enemyPaperSprite.SetActive(false);
        _enemyScissorsSprite.SetActive(false);
        _enemyUnknownSprite.SetActive(true);
    }

    IEnumerator EnemyTurn()
    {
        string enemyChoice = Choices[Random.Range(0, Choices.Length)];

        if (enemyChoice == "Rock")
        {
            enemyUnit.CurrentHand = "Rock";
            HandManager.PlayHand("Rock");
            _enemyRockSprite.SetActive(true);
            _enemyPaperSprite.SetActive(false);
            _enemyScissorsSprite.SetActive(false);
            _enemyUnknownSprite.SetActive(false);
        }

        else if (enemyChoice == "Paper")
        {
            enemyUnit.CurrentHand = "Paper";
            HandManager.PlayHand("Paper");
            _enemyRockSprite.SetActive(false);
            _enemyPaperSprite.SetActive(true);
            _enemyScissorsSprite.SetActive(false);
            _enemyUnknownSprite.SetActive(false);
        }

        else if (enemyChoice == "Scissors")
        {
            enemyUnit.CurrentHand = "Scissors";
            HandManager.PlayHand("Scissors");
            _enemyRockSprite.SetActive(false);
            _enemyPaperSprite.SetActive(false);
            _enemyScissorsSprite.SetActive(true);
            _enemyUnknownSprite.SetActive(false);
        }

        yield return new WaitForSeconds(1f);

        state = BattleState.BATTLEPHASE;
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
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
            SceneManager.LoadScene("End");
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated.";
        }
    }

    #endregion

    #region Player Choice

    public void RockButton()
    {
        if (state != BattleState.PLAYERTURN && _playerUnit.CurrentHand != "Unknown")
        {
            return;
        }

        HandManager.PlayHand("Rock");
        dialogueText.text = "You have chosen rock";
        _playerUnit.CurrentHand = "Rock";

        _playerRockSprite.SetActive(true);
        _playerPaperSprite.SetActive(false);
        _playerScissorsSprite.SetActive(false);
        _playerUnknownSprite.SetActive(false);

        StartCoroutine(PlayerHand());
    }

    public void PaperButton()
    {
        if (state != BattleState.PLAYERTURN && _playerUnit.CurrentHand != "Unknown")
        {
            return;
        }

        HandManager.PlayHand("Paper");
        dialogueText.text = "You have chosen paper";
        _playerUnit.CurrentHand = "Paper";

        _playerRockSprite.SetActive(false);
        _playerPaperSprite.SetActive(true);
        _playerScissorsSprite.SetActive(false);
        _playerUnknownSprite.SetActive(false);

        StartCoroutine(PlayerHand());
    }

    public void ScissorsButton()
    {
        if (state != BattleState.PLAYERTURN && _playerUnit.CurrentHand != "Unknown")
        {
            return;
        }

        HandManager.PlayHand("Scissors");
        dialogueText.text = "You have chosen scissors";
        _playerUnit.CurrentHand = "Scissors";

        _playerRockSprite.SetActive(false);
        _playerPaperSprite.SetActive(false);
        _playerScissorsSprite.SetActive(true);
        _playerUnknownSprite.SetActive(false);

        StartCoroutine(PlayerHand());
    }

    IEnumerator PlayerHand()
    {
        state = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemyTurn());
    }

    #endregion 

    #region Display Utility

    public IEnumerator PlayerDamagedFlash() //IEnumerator is useful to add delays
    {
        while (_playerIsDamaged)
        {
            _playerSpriteRenderer.color = new Color(1f, 1f, 1f, 0f); //Flashing animation from max opacity to none (r,g,b,a)
            yield return new WaitForSeconds(_playerDamagedFlashDelay);
            _playerSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(_playerDamagedFlashDelay);
        }
    }

    public IEnumerator HandlePlayerDamagedDelay() //Timer on the player damaged state
    {
        yield return new WaitForSeconds(_playerDamagedTimeDelay);
        _playerIsDamaged = false;
    }

    #endregion
}
