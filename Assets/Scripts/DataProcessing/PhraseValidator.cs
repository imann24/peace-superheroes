using System.Collections;
using System.Collections.Generic;

public class PhraseValidator {
	private const string GREAT_PHRASE_FEEDBACK = "That was an excellent choice";
	private const string GOOD_PHRASE_FEEDBACK = "That phrase worked";
	private const string BAD_PHRASE_FEEDBACK = "That was a poor choice of phrases";

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

	public static string GetFeedback (Quality quality, string response, string conflict) {
		switch (quality) {

		case Quality.Great:
			return GREAT_PHRASE_FEEDBACK;
		case Quality.Good:
			return GOOD_PHRASE_FEEDBACK;
		case Quality.Bad:
			return BAD_PHRASE_FEEDBACK;
		default:
			return GOOD_PHRASE_FEEDBACK;
		
		}
	}

}
