using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseController : MonoBehaviour {
	public static PhraseController Instance;

	public TextAsset CorrectPhrasesDocument;
	public TextAsset AllPhrasesDocument;

	private string [] correctPhrases;
	private string [] allPhrases;

	public const string DEFAULT_PHRASE = "collectible";
	public bool PseudoRandomPhraseGeneration = true;

	private Queue <string> pseudoRandomPhraseSpawnOrder = new Queue<string>();

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
		generatePseudoRandomPhrases();
	}

	public string GetRandomPhrase () {
		if (PseudoRandomPhraseGeneration) { 
			return GetPsuedoRandomPhrase();
		} else {
			return GetTrueRandomPhrase();
		}
	}

	public string GetTrueRandomPhrase () {
		return allPhrases[Random.Range(0, allPhrases.Length)];
	}

	public string GetPsuedoRandomPhrase () {
		if (pseudoRandomPhraseSpawnOrder.Count == 0) {
			Debug.LogError("Pseudo random order has not been initialized");
			return GetTrueRandomPhrase();
		} else {
			string nextPhrase = pseudoRandomPhraseSpawnOrder.Dequeue();
			pseudoRandomPhraseSpawnOrder.Enqueue(nextPhrase);
			return nextPhrase;
		}
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

	private void generatePseudoRandomPhrases () {
		while (pseudoRandomPhraseSpawnOrder.Count < allPhrases.Length) {
			string randomPhrase = GetTrueRandomPhrase();
			if (pseudoRandomPhraseSpawnOrder.Contains(randomPhrase)) {
				continue;
			} else {
				pseudoRandomPhraseSpawnOrder.Enqueue(randomPhrase);
			}
		}
	}
}
