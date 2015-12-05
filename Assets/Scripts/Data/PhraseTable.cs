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
}
