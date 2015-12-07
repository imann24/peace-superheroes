using System;
using System.Collections;

public class PhraseTable {
	private string [] responsePhrases;
	private string [] conflictPhrases;
	private int [,] phraseRatings;

	public PhraseTable (string [] responsePhrases, string [] conflictPhrases, int[,] phraseRatings) {
		this.responsePhrases = responsePhrases;
		this.conflictPhrases = conflictPhrases;
		this.phraseRatings = phraseRatings;
	}

	public Quality GetRating (string response, string conflict) {
		return getQualityFromIntRating(
			getRatingFromPhrases(response, conflict));
	}

	private Quality getQualityFromIntRating (int rating) {
		return ((Quality) rating-1);
	}

	private int getRatingFromPhrases (string response, string conflict) {
		return phraseRatings[
		                     Array.IndexOf(responsePhrases, response),
		                     Array.IndexOf(conflictPhrases, conflict)
		                     ];
	}

	public override string ToString ()
	{
		string phraseTableAsString = "Conflicts:\n" + String.Join(", ", conflictPhrases) + "\n";

		phraseTableAsString += "Responses:\n" + string.Join(", ", responsePhrases) + "\n";

		phraseTableAsString += "Ratings:\n" + Util.TwoDimensionArrayToString(phraseRatings);
		return phraseTableAsString;
	}

	public string [] GetResponses () {
		return responsePhrases;
	}

	public string GetRandomResponse () {
		return Util.RandomElement(responsePhrases);
	}

	public string [] GetConflicts () {
		return conflictPhrases;
	}

	public string GetRandomConflict () {
		return Util.RandomElement(conflictPhrases);
	}
}
