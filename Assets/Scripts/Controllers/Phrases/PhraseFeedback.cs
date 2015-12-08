using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseFeedback : MonoBehaviour {
	public delegate void FeedbackClosedAction(Quality quality);
	public static event FeedbackClosedAction OnFeedbackClosed;
	public static PhraseFeedback Instance;
	public Text Feedback;

	private Quality qualityOfLastResponse;

	void Awake () {
		Instance = this;
	}

	void OnDestroy () {
		Instance = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateFeedback (string feedback, Quality quality) {
		gameObject.SetActive(true);
		Feedback.text = feedback;
		setQualityOfLastResponse(quality);
	}

	public void DeactivateFeedback (bool unpauseGame = true) {
		gameObject.SetActive(false);
		if (unpauseGame) {
			MovementController.Instance.Paused = false;

		}
		callFeedbackClosedEvent();
	}

	private void setQualityOfLastResponse (Quality quality) {
		qualityOfLastResponse = quality;
	}

	private void callFeedbackClosedEvent () {
		if (OnFeedbackClosed != null) {
			OnFeedbackClosed(qualityOfLastResponse);
		}
	}
}
