using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]

public class TrackerBarController : MonoBehaviour {
	Image image;
	Score score;

	Color calmColor = Color.green;
	Color madColor = Color.red;

	private IEnumerator barLengthChangeCoroutine;

	// Use this for initialization
	void Awake () {
		setReferences();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setReferences () {
		image = GetComponent<Image>();
	}

	void setBarLength (float scoreFraction) {
		startBarLerpCoroutine(scoreFraction);
	}

	public void SetScore (Score score) {
		this.score = score;
		score.OnScoreChange += setBarLength;
		setBarLength(score.GetScoreFraction());
	}

	void stopCurrentBarLerpCoroutine () {
		if (barLengthChangeCoroutine != null) {
			StopCoroutine(barLengthChangeCoroutine);
		}
	}

	void startBarLerpCoroutine (float targetLength) {
		stopCurrentBarLerpCoroutine();

		barLengthChangeCoroutine = LerpBarLength(targetLength);

		StartCoroutine(barLengthChangeCoroutine);
	}

	IEnumerator LerpBarLength (float targetLength, float maxTime = Global.DONE_TIME) {
		float startLength = image.fillAmount;
		float timer = 0;

		while (timer < maxTime) {
			if (!MovementController.Instance.Paused) {
				image.fillAmount = Mathf.Lerp(startLength,
				                             targetLength,
				                              timer);
				timer += Time.deltaTime;
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
