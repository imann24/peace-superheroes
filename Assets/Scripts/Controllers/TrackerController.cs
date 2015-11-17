using UnityEngine;
using System.Collections;

public class TrackerController : MonoBehaviour {
	public int startingValue = 0;
	private Score score;
	public TrackerBarController trackerBar;
	public PhraseCountController phraseCount;

	public GameObject WinScreen;
	public GameObject LoseScreen;

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

	void initialize () {
		score = new Score(startingValue);
		trackerBar.SetScore(score);
		phraseCount.SetScore(score);
		subscribeEvents();
	}

	void scoreNPCEncounter (Emotion emotion) {
		switch (emotion) {
			case Emotion.Calm:
				score.CollectPhrase();
				break;
			case Emotion.Mad:
				score.AngryEncounter();
				break;
			default:
				break;
		}
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
		score.OnGameStateChange += loadScreen;
	}

	void unsubscribeEvents () {
		NPCSpawnController.OnNPCEncounter -= scoreNPCEncounter;
		score.OnGameStateChange -= loadScreen;
	}
}
