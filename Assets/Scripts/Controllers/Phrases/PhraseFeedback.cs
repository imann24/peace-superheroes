using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseFeedback : MonoBehaviour {
	public delegate void FeedbackClosedAction(Quality quality);
	public static event FeedbackClosedAction OnFeedbackClosed;
	public static PhraseFeedback Instance;
	public Text Feedback;

	public Image SpeechBubble;
	public Color GoodFeedbackColor;
	public Color BadFeedbackColor;

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
		setFeedbackColor(quality);
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

	private void setFeedbackColor (Quality quality) {
		if (quality == Quality.Good ||
		    quality == Quality.Great) {

			StartCoroutine(LerpColor(
				BadFeedbackColor,
				GoodFeedbackColor));


		} else if (quality == Quality.Bad) {

			SpeechBubble.color = BadFeedbackColor;

		}
	}

	IEnumerator LerpColor (Color startColor, Color targetColor, float speed = 0.5f) {
		float timer = 0;

		while (timer <= 1) {

			SpeechBubble.color = Color.Lerp(
				startColor,
				targetColor,
				timer);

			timer += Time.deltaTime * speed;

			yield return new WaitForEndOfFrame();
		}
	}
}
