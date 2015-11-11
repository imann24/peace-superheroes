using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
	public Text ScoreValue;
	public CanvasGroup VictoryScreen;

	// Use this for initialization
	void Start () {
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	void setScoreValue (int score) {
		ScoreValue.text = score.ToString();
	}

	void subscribeEvents () {
		PlayerController.OnVictory += activateVictoryCanvas;
		ScoreController.OnUpdateScore += setScoreValue;
	}

	void unsubscribeEvents () {
		PlayerController.OnVictory -= activateVictoryCanvas;
		ScoreController.OnUpdateScore -= setScoreValue;
	}

	private void activateVictoryCanvas () {
		toggleCanvasGroup(VictoryScreen, true);
	}
	private void toggleCanvasGroup (CanvasGroup canvasGroup, bool active) {
		canvasGroup.interactable = active;
		canvasGroup.blocksRaycasts = active;
		canvasGroup.alpha = active?1.0f:0.0f;
	}


}
