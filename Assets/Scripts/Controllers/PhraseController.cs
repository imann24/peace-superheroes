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
	void Start () {
		Util.SingletonImplementation(
			ref Instance,
			this,
			gameObject);

		if (correctPhrases != null) {
			correctPhrases = LineReader.ReadByLine(CorrectPhrasesDocument);
		}

		if (allPhrases != null) {
			allPhrases = LineReader.ReadByLine(AllPhrasesDocument);
		}

		addValidPhrases();
	}
	
	public string GetRandomPhrase () {
		return allPhrases[Random.Range(0, allPhrases.Length)];
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
