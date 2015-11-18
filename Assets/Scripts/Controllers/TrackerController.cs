using UnityEngine;
using System.Collections;

public class TrackerController : MonoBehaviour {

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

	void scoreNPCEncounter (Emotion emotion) {
		switch (emotion) {
			case Emotion.Mad:
				score.AngryEncounter();
				break;
			default:
				break;
		}
	}

	void collectPhrase (string phrase) {
		score.CollectPhrase();
		PhraseCollector.Instance.CollectPhrase(phrase);
	}

	void loadScreen (GameState gameState) {
		LoseScreen.SetActive(gameState == GameState.GameLose);
		WinScreen.SetActive(gameState == GameState.GameWin);

		if (gameState == GameState.GameLose ||
		    gameState == GameState.GameWin) {

			Global.Paused = true;
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
}
