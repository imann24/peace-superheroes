using UnityEngine;
using System.Collections;

public class LevelParser {
	private const string PlatformKey = "x";
	private const string PlayerKey = "p";
	private const string GroundKey = "g";
	private const string FinishKey = "f";
	private const string CollectibleKey = "c";

	public static LevelPiece [,] ParseLevel (string [][] levelAsStrings) {
		int height = Util.MatrixHeight(levelAsStrings);
	
		LevelPiece [,] level = new LevelPiece[levelAsStrings.Length,height];

		for (int x = 0; x < levelAsStrings.Length; x++) {
			for (int y = 0; y < height; y++) {
				level[x,y] = ParsePiece(levelAsStrings[x][y]);
			}
		}

		return level;
	}

	private static LevelPiece ParsePiece (string piece) {
		switch (piece) {
			case PlatformKey:
				return LevelPiece.Platform;
			case PlayerKey:
				return LevelPiece.Player;
			case GroundKey:
				return LevelPiece.Ground;
			case FinishKey:
				return LevelPiece.Finish;
			case CollectibleKey:
				return LevelPiece.Collectible;
			default:
				return LevelPiece.Empty;
		}
	}
}
