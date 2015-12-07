using UnityEngine;
using System.Collections;

public class PhraseTableParser {

	public PhraseTable Parse (string[][] csv) {
		int responseCount = csv.Length - 1;
		int conflictCount = csv[0].Length - 1;
		string [] responses = new string [responseCount];
		string [] conflicts = new string [conflictCount];
		int [,] ratings = new int[responseCount, conflictCount];

		for (int i = 0; i < Mathf.Max(responseCount, conflictCount); i++) {
			if (i < responseCount) {
				responses[i] = csv[i+1][0];
			}

			if (i < conflictCount) {
				conflicts[i] = csv[0][i+1];
			}

			for (int j = 0; j < Mathf.Min(responseCount, conflictCount); j++) {
				bool inverse = responseCount < conflictCount;

				int x = inverse ? j : i;
				int y = inverse ? i : j;

				try {
					ratings[x, y] = int.Parse(csv[x+1][y+1]);
				} catch {
					Debug.LogError("Rating (" + (x+1) + ", " + (y+1) + ")" + csv[x+1][y+1] + "***" + " is not an integer; returning null");
					return null;
				}
			}
		}

		return new PhraseTable(responses, conflicts, ratings);
	}
}
