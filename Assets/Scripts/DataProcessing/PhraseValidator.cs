using System.Collections;
using System.Collections.Generic;

public class PhraseValidator {
	private static List<string> correctPhrases = new List<string>();

	public static bool PhraseCorrect (string phrase) {
		return correctPhrases.Contains(phrase);
	}

	public static void AddCorrectPhrase (string phrase) {
		correctPhrases.Add(phrase);
	}

	public static void SetCorrectPhrases (List<string> correctPhrases) {
		PhraseValidator.correctPhrases = correctPhrases;
	}

}
