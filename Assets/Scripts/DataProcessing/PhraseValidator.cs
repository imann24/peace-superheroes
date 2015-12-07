using System.Collections;
using System.Collections.Generic;

public class PhraseValidator {
	private const string GREAT_PHRASE_FEEDBACK = "That was an excellent choice";
	private const string GOOD_PHRASE_FEEDBACK = "That phrase worked";
	private const string BAD_PHRASE_FEEDBACK = "That was a poor choice of phrases";

	private const int GREAT_PHRASE_POINTS = 2;
	private const int GOOD_PHRASE_POINTS = 1;
	private const int BAD_PHRASE_POINTS = -1;

	private static List<string> correctPhrases = new List<string>();

	private static int PhraseMax = 5;

	public static bool CanAcceptPhrase (string phrase = null) {
		return PhraseCollector.Instance.GetPhraseCount() < PhraseMax;
	}

	public static bool PhraseCorrect (string phrase) {
		return correctPhrases.Contains(phrase);
	}
	
	public static void AddCorrectPhrase (string phrase) {
		correctPhrases.Add(phrase);
	}

	public static void SetCorrectPhrases (List<string> correctPhrases) {
		PhraseValidator.correctPhrases = correctPhrases;
	}

	public static string GetFeedback (Quality quality, string response = null, string conflict = null) {
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

	public static int GetPoints (Quality quality, string response = null, string conflict = null) {
		switch (quality) {
			
		case Quality.Great:
			return GREAT_PHRASE_POINTS;
		case Quality.Good:
			return GOOD_PHRASE_POINTS;
		case Quality.Bad:
			return BAD_PHRASE_POINTS;
		default:
			return GOOD_PHRASE_POINTS;
			
		}
	}

}
