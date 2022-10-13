using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, BATTLEPHASE, WON, LOST }

public class BattleSystem : MonoBehaviour
{
	public GameObject playerPrefab;
	public GameObject enemyPrefab;
	public Transform playerBattleStation;
	public Transform enemyBattleStation;
	Unit playerUnit;
	Unit enemyUnit;
	public Text dialogueText;
	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;
	public BattleState state;
	public HandManager HandManager;

	[Header("Player Variables")]
	[SerializeField] private GameObject _PlayerButtons;
	private float _playerDamagedTimeDelay = 1f;
	private float _playerDamagedFlashDelay = 0.1f;
	private bool _playerIsDamaged = false;
	[SerializeField] private SpriteRenderer _playerSpriteRenderer;
	[SerializeField] private GameObject _playerRockSprite;
	[SerializeField] private GameObject _playerPaperSprite;
	[SerializeField] private GameObject _playerScissorsSprite;
	[SerializeField] private GameObject _playerUnknownSprite;

	[Header("Enemy Variables")]
	public string[] Choices;
	[SerializeField] private SpriteRenderer _enemySpriteRenderer;
	[SerializeField] private GameObject _enemyRockSprite;
	[SerializeField] private GameObject _enemyPaperSprite;
	[SerializeField] private GameObject _enemyScissorsSprite;
	[SerializeField] private GameObject _enemyUnknownSprite;

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
		playerUnit = playerGO.GetComponent<Unit>();

		GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
		enemyUnit = enemyGO.GetComponent<Unit>();

		dialogueText.text = enemyUnit.UnitName + " is ready!";

		playerHUD.SetHUD(playerUnit);
		enemyHUD.SetHUD(enemyUnit);

		yield return new WaitForSeconds(2f);

		_PlayerButtons.SetActive(true);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	IEnumerator PlayerAttack()
	{
		bool isDead = enemyUnit.TakeDamage(playerUnit.Damage);

		yield return new WaitForSeconds(1f);

		enemyHUD.SetHP(enemyUnit.CurrentHP);
		dialogueText.text = playerUnit.UnitName + " has won this round!";

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.WON;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}
	}

	IEnumerator EnemyAttack()
	{
		dialogueText.text = enemyUnit.UnitName + " has won this round...";

		yield return new WaitForSeconds(1f);

		bool isDead = playerUnit.TakeDamage(enemyUnit.Damage);

		playerHUD.SetHP(playerUnit.CurrentHP);

		_playerIsDamaged = true;
		StartCoroutine(PlayerDamagedFlash());
		StartCoroutine(HandlePlayerDamagedDelay());

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		} else
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

		if (playerUnit.CurrentHand == "Rock" && enemyUnit.CurrentHand == "Scissors")
		{
			StartCoroutine(PlayerAttack());
		}

		else if (playerUnit.CurrentHand == "Paper" && enemyUnit.CurrentHand == "Rock")
		{
			StartCoroutine(PlayerAttack());
		}

		else if (playerUnit.CurrentHand == "Scissors" && enemyUnit.CurrentHand == "Paper")
		{
			StartCoroutine(PlayerAttack());
		}

		else if (playerUnit.CurrentHand == "Rock" && enemyUnit.CurrentHand == "Paper")
		{
			StartCoroutine(EnemyAttack());
		}

		else if (playerUnit.CurrentHand == "Paper" && enemyUnit.CurrentHand == "Scissors")
		{
			StartCoroutine(EnemyAttack());
		}

		else if (playerUnit.CurrentHand == "Scissors" && enemyUnit.CurrentHand == "Rock")
		{
			StartCoroutine(EnemyAttack());
		}

		else 
		{
			StartCoroutine(Tiebreaker());
		}
	}

	void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
			SceneManager.LoadScene("End");
		} 
		else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
		playerUnit.CurrentHand = "Unknown";
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

	IEnumerator PlayerHand()
	{
		state = BattleState.ENEMYTURN;
		yield return new WaitForSeconds(1f);
		StartCoroutine(EnemyTurn());
	}

	public void RockButton()
	{
		if (state != BattleState.PLAYERTURN && playerUnit.CurrentHand != "Unknown")
		{
			return;
		}

		HandManager.PlayHand("Rock");
		dialogueText.text = "You have chosen rock";
		playerUnit.CurrentHand = "Rock";
		_playerRockSprite.SetActive(true);
		_playerPaperSprite.SetActive(false);
		_playerScissorsSprite.SetActive(false);
		_playerUnknownSprite.SetActive(false);

		StartCoroutine(PlayerHand());
	}

	public void PaperButton()
	{
		if (state != BattleState.PLAYERTURN && playerUnit.CurrentHand != "Unknown")
		{
			return;
		}

		HandManager.PlayHand("Paper");
		dialogueText.text = "You have chosen paper";
		playerUnit.CurrentHand = "Paper";
		_playerRockSprite.SetActive(false);
		_playerPaperSprite.SetActive(true);
		_playerScissorsSprite.SetActive(false);
		_playerUnknownSprite.SetActive(false);

		StartCoroutine(PlayerHand());
	}

	public void ScissorsButton()
	{
		if (state != BattleState.PLAYERTURN && playerUnit.CurrentHand != "Unknown")
		{
			return;
		}

		HandManager.PlayHand("Scissors");
		dialogueText.text = "You have chosen scissors";
		playerUnit.CurrentHand = "Scissors";
		_playerRockSprite.SetActive(false);
		_playerPaperSprite.SetActive(false);
		_playerScissorsSprite.SetActive(true);
		_playerUnknownSprite.SetActive(false);		

		StartCoroutine(PlayerHand());
	}

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

}
