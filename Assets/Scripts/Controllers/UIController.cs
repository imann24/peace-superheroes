using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
	public Text ScoreValue;

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

	void setScoreValue (int score) {
		ScoreValue.text = score.ToString();
	}

	void subscribeEvents () {
		ScoreController.OnUpdateScore += setScoreValue;
	}

	void unsubscribeEvents () {
		ScoreController.OnUpdateScore -= setScoreValue;
	}
}
