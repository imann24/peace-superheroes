using UnityEngine;
using System.Collections;

public class PhraseController : MonoBehaviour {
	public static PhraseController Instance;

	public TextAsset CorrectPhrasesDocument;
	public TextAsset AllPhrasesDocument;

	private string [] correctPhrases;
	private string [] allPhrases;

	public const string DEFAULT_PHRASE = "collectible";

	// Use this for initialization
	void Awake () {
		Util.SingletonImplementation(
			ref Instance,
			this,
			gameObject);

		if (CorrectPhrasesDocument != null) {
			correctPhrases = LineReader.ReadByLine(CorrectPhrasesDocument);
		}

		if (AllPhrasesDocument != null) {
			allPhrases = LineReader.ReadByLine(AllPhrasesDocument);
		}
		addValidPhrases();
	}

	public string GetRandomPhrase () {
		return allPhrases[Random.Range(0, allPhrases.Length)];
	}

	public string GetPhrase (int index) {
		if (index < allPhrases.Length) {
			return allPhrases[index];
		} else {
			return "";
		}
	}
	
	private void addValidPhrases () {
		PhraseValidator.AddCorrectPhrase(DEFAULT_PHRASE);

		if (correctPhrases != null) {
			for (int i = 0; i < correctPhrases.Length; i++) {
				PhraseValidator.AddCorrectPhrase(correctPhrases[i]);
			}
		}
	}
}
