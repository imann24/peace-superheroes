using UnityEngine;
using System.Collections;

public class TrackerController : MonoBehaviour {
	public delegate void GameEndAction();
	public static event GameEndAction OnGameEnd;

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
		PhraseCollector.Instance.CollectPhrase(phrase);
	}

	public int PhraseCount () {
		return score.PhraseCount;
	}

	public void UsePhraseCorrectly () {
		score.SetScore(score.GetScore() + 1);
		score.PhraseCount--;
	}

	void loadScreen (GameState gameState) {
		LoseScreen.SetActive(gameState == GameState.GameLose);
		WinScreen.SetActive(gameState == GameState.GameWin);

		if (gameState == GameState.GameLose ||
		    gameState == GameState.GameWin) {
			callGameEndEvent();
			MovementController.Instance.Paused = true;
		}
	}

	void subscribeEvents () {
		NPCSpawnController.OnNPCEncounter += scoreNPCEncounter;
		Phrase.OnPhraseCollected += collectPhrase;
		score.OnGameStateChange += loadScreen;
	}

	void unsubscribeEvents () {
		NPCSpawnController.OnNPCEncounter -= scoreNPCEncounter;
		Phrase.OnPhraseCollected -= collectPhrase;
	}

	void callGameEndEvent () {
		if (OnGameEnd != null) {
			OnGameEnd();
		}
	}
}
