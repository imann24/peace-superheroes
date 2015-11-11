// Data structure to store a level

using UnityEngine;
using System.Collections;

public class Level {

	// Stores the level as a 2d array
	private LevelPiece[,] levelPieces;

	// Creates level from the pieces
	public Level (LevelPiece[,] levelPieces) {
		this.levelPieces = levelPieces;
	}

	// Returns the level pieces
	public LevelPiece[,] GetLevelPieces () {
		return levelPieces;
	}

	// Sets an individual piece
	public void SetLevelPiece (int x, int y, LevelPiece type) {
		this.levelPieces[x,y] = type;
	}

	// Gets height
	public int GetHeight () {
		return levelPieces.GetLength(1);
	}

	// Gets width
	public int GetWidth () {
		return levelPieces.GetLength(0);
	}
}
