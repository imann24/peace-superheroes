using UnityEngine;
using System.Collections;

public class LevelPositioner {
	private static float xScaler = 4.0f;
	private static float yScaler = 1.75f;

	public static Vector2 PositionPiece (int x, int y) {
		return new Vector2(x * xScaler, y * yScaler);
	}
}
