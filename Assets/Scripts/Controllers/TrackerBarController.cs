using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Image))]

public class TrackerBarController : MonoBehaviour {
	Image image;
	Score score;

	Color calmColor = Color.green;
	Color madColor = Color.red;
	
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
		image.fillAmount = scoreFraction;
	}

	public void SetScore (Score score) {
		this.score = score;
		score.OnScoreChange += setBarLength;
		setBarLength(score.GetScoreFraction());
	}
}
