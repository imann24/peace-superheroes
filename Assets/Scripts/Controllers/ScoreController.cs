using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour {
	public static ScoreController Instance;

	public delegate void UpdateScoreAction (int currentScore);
	public static event UpdateScoreAction OnUpdateScore;

	private int multiplier = 1;
	private int score = 0;
	private int defaultScoreIncrease = 10;

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
	}
	// Use this for initialization
	void Start () {
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetMultiplier (int multiplier) {
		this.multiplier = multiplier;
	}

	void subscribeEvents () {
		LevelPieceController.OnCollect += checkForValidCollectible;
		PlayerController.OnPlayerOutOfBounds += resetScore;
	}

	void unsubscribeEvents () {
		LevelPieceController.OnCollect -= checkForValidCollectible;
		PlayerController.OnPlayerOutOfBounds -= resetScore;
	}

	void checkForValidCollectible (string collectible) {
		if (PhraseValidator.PhraseCorrect(collectible)) {
			score += defaultScoreIncrease * multiplier;
			callOnUpdateScore();
		}
	}

	void resetScore () {
		score = 0;
		callOnUpdateScore();
	}

	void callOnUpdateScore () {
		if (OnUpdateScore != null) {
			OnUpdateScore(score);
		}
	}

}
