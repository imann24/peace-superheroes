using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Text))]

public class PhraseCountController : MonoBehaviour {
	Score score;
	Text text;
	const string PHRASE_TEXT = "Phrases: ";

	// Use this for initialization
	void Start () {
		setReferences();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void updatePhraseCount (int phraseCount) {
		text.text = PHRASE_TEXT + phraseCount;
	}

	public void SetScore (Score score) {
		this.score = score;
		this.score.OnPhraseCountChange += updatePhraseCount;
	}

	void setReferences () {
		text = GetComponent<Text>();
	}
}
