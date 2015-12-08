using UnityEngine;
using System.Collections;

public class TrackerController : MonoBehaviour {
	public delegate void GameEndAction(bool victory);
	public static event GameEndAction OnGameEnd;

	public delegate void CharacterReaction(Quality quality);
	public static event CharacterReaction 
		OnCharacterReaction;
	public static TrackerController Instance;

	public int startingValue = 0;
	private Score score;
	public TrackerBarController trackerBar;
	public PhraseCountController phraseCount;

	public GameObject WinScreen;
	public GameObject LoseScreen;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		initialize();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded (int level) {
		loadScreen(GameState.Game);

	}

	void initialize () {
		resetScore();
		subscribeEvents();
	}

	void resetScore () {
		score = new Score(startingValue);
		trackerBar.SetScore(score);
		phraseCount.SetScore(score);
	}

	void scoreNPCEncounter (Emotion emotion, string phrase = null) {
		switch (emotion) {
			case Emotion.Mad:
				score.AngryEncounter();
				break;
			default:
				break;
		}
	}

	public void collectPhrase (string phrase) {
		score.CollectPhrase();
	}

	public int PhraseCount () {
		return score.PhraseCount;
	}

	public void UsePhrase (int scoreChange) {
		score.SetScore(score.GetScore() + scoreChange);
		score.PhraseCount--;
	}

	public void AngryEncounterWithoutPhrase (int scoreLost = -1) {
		score.SetScore(score.GetScore() + scoreLost);
		callCharacterReactionEvent(Quality.Bad);

	}

	void loadScreen (GameState gameState) {
		LoseScreen.SetActive(gameState == GameState.GameLose);
		WinScreen.SetActive(gameState == GameState.GameWin);

		if (gameState == GameState.GameLose ||
		    gameState == GameState.GameWin) {
			callGameEndEvent(gameState == GameState.GameWin);
			MovementController.Instance.Paused = true;
		}
	}

	void subscribeEvents () {
		Phrase.OnPhraseCollected += collectPhrase;
		score.OnGameStateChange += loadScreen;
	}

	void unsubscribeEvents () {
		Phrase.OnPhraseCollected -= collectPhrase;
	}

	void callGameEndEvent (bool victory) {
		if (OnGameEnd != null) {
			OnGameEnd(victory);
		}
	}

	void callCharacterReactionEvent (Quality quality) {
		if (OnCharacterReaction != null) {
			OnCharacterReaction(quality);
		}
	}
}
