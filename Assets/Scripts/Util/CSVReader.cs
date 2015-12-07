using UnityEngine;
using System.Collections;

public class CSVReader {
	
	public static string [][] ParseTSV (TextAsset TSV) {
		return ParseCSV(TSV, '\t');
	}

	// Returns a 2d array of the CSV
	public static string [][] ParseCSV (TextAsset CSV, char separator = ',', bool removeSpaces = true) {
		if (CSV == null) {
			return null;
		}



		string[] CSVByLine = CSV.text.Split('\n');
		int width = determineMaxWidth(
			CSVByLine,
			separator);

		string[][] CSVByCell = new string[width][];

		for (int x = 0; x < CSVByCell.Length; x++) {
			CSVByCell[x] = new string[CSVByLine.Length];
		}

		for (int y = 0; y < CSVByLine.Length; y++) {
			
			string [] lineByCell = CSVByLine[y].Split(separator);
			
			for (int x = 0; x < CSVByCell.Length; x++) {
				if (x < lineByCell.Length) {
					CSVByCell[x][y] = Util.RemoveSpaces(lineByCell[x]);
				}
			}
		}

		return CSVByCell;
	}

	// Determines the max width of an array of strings
	private static int determineMaxWidth (string[] lines, char splitChar = ',') {
		int max = 0;
		
		for (int i = 0; i < lines.Length; i++) {
			max =
				max < lines[i].Split(splitChar).Length ?
					max = lines[i].Split(splitChar).Length :
					max = max;
		}
		
		return max;
	}

}
