using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Score {

	public Score (int startingScore = 0) {
		this._score = startingScore;
	}

	public delegate void GameStateChangeAction (GameState newState);
	public event GameStateChangeAction OnGameStateChange;

	public delegate void ScoreChangeAction (float scoreFraction);
	public event ScoreChangeAction OnScoreChange;

	public delegate void PhraseCountChangeAction (int phraseCount);
	public event PhraseCountChangeAction OnPhraseCountChange;

	private const int LOSE = -5;
	private const int WIN = 10;

	private List<string> CurrentPhrases = new List<string>();

	int _score;
	int _phraseCount = 0;

	public int PhraseCount {
		get {
			return _phraseCount;
		}

		set {
			_phraseCount = value;
			callPhraseCountChangeEvent();
		}
	}

	private void callGameStateChangeEvent (GameState newState) {
		if (OnGameStateChange != null) {
			OnGameStateChange(newState);
		}
	}

	private void callScoreChangeEvent () {
		if (OnScoreChange != null) {
			OnScoreChange(GetScoreFraction());
		}
	}

	private void callPhraseCountChangeEvent () {
		if (OnPhraseCountChange != null) {
			OnPhraseCountChange(_phraseCount);
		}
	}

	public void SetScore (int score) {
		if (_score >= WIN) {
			callGameStateChangeEvent(GameState.GameWin);
		} else if (_score <= LOSE) {
			callGameStateChangeEvent(GameState.GameLose);
		} else {
			_score = score;
			callScoreChangeEvent();
		}
	}

	public int GetScore () {
		return _score;
	}

	public float GetScoreFraction () {
		int startOffset = -LOSE;
		int range = WIN + startOffset;
		return (float) (_score + startOffset) / (float) range;
	}

	public void CollectPhrase () {
		PhraseCount++;
	}

	public void AngryEncounter () {
		if (PhraseCollector.Instance.GetPhraseCount() == 0) {
			SetScore(_score - 1);
		}
	}
}
