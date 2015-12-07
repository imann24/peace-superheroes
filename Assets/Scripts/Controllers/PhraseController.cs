using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PhraseController : MonoBehaviour {
	public static PhraseController Instance;

	public delegate string PhraseGenerationAction();

	public TextAsset CorrectPhrasesDocument;
	public TextAsset AllPhrasesDocument;
	public TextAsset PhraseRatingsDocument;

	private string [] correctPhrases;
	private string [] allPhrases;
	private PhraseTable phraseTable;

	public const string DEFAULT_PHRASE = "collectible";
	public bool PseudoRandomPhraseGeneration = true;

	private Queue <string> pseudoRandomPhraseSpawnOrder;
	private Queue <string> pseudoRandomConflictPhraseSpawnOrder;
	// Use this for initialization
	void Awake () {
		initialize();

	}

	public string GetRandomPhrase () {
		if (PseudoRandomPhraseGeneration) { 
			return GetPsuedoRandomPhrase(pseudoRandomPhraseSpawnOrder);
		} else {
			return GetTrueRandomPhrase();
		}
	}

	public string GetTrueRandomPhrase () {
		return phraseTable.GetRandomResponse();
	}

	public string GetPsuedoRandomPhrase (Queue<string> pseudoRandomPhraseSpawnOrder) {
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

	public string GetRandomConflictPhrase () {
		if (PseudoRandomPhraseGeneration) { 
			return GetPsuedoRandomPhrase(pseudoRandomConflictPhraseSpawnOrder);
		} else {
			return phraseTable.GetRandomConflict();
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

	private void initializePhrases () {
		phraseTable = new PhraseTableParser().Parse(
			CSVReader.ParseCSV(PhraseRatingsDocument));


		correctPhrases = phraseTable.GetResponses();
		allPhrases = phraseTable.GetResponses();
		addValidPhrases();
	}

	private void initializePseudoRandomPhrases() {

		pseudoRandomPhraseSpawnOrder = generatePseudoRandomPhrases(
			phraseTable.GetRandomResponse,
			phraseTable.GetResponses());

		pseudoRandomConflictPhraseSpawnOrder = generatePseudoRandomPhrases(
			phraseTable.GetRandomConflict,
			phraseTable.GetConflicts());
	}

	private void initialize () {
		Util.SingletonImplementation(
			ref Instance,
			this,
			gameObject);

		initializePhrases();
		initializePseudoRandomPhrases();
	}

	private Queue<string> generatePseudoRandomPhrases (PhraseGenerationAction phraseGenerator, string[] allPhrases) {
		Queue<string> pseudoRandomPhraseSpawnOrder = new Queue<string>();

		while (pseudoRandomPhraseSpawnOrder.Count < allPhrases.Length) {
			string randomPhrase = phraseGenerator();
			if (pseudoRandomPhraseSpawnOrder.Contains(randomPhrase)) {
				continue;
			} else {
				pseudoRandomPhraseSpawnOrder.Enqueue(randomPhrase);
			}
		}

		return pseudoRandomPhraseSpawnOrder;
	}
}
