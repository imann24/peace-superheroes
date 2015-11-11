using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

	public static Level [] GenerateLevels (TextAsset[] CSVs) {

		Level [] levels = new Level[CSVs.Length];

		for (int i = 0; i < CSVs.Length; i++) {

			string[][] levelAsString = CSVReader.ParseCSV(CSVs[i]);

			LevelPiece[,] levelPieces = LevelParser.ParseLevel(levelAsString);

			levels[i] = new Level(levelPieces);
		}

		return levels;
	}
}
