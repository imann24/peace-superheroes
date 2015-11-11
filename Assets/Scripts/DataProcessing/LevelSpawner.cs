using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {

	public GameObject PlatformPrefab;
	public GameObject PlayerPrefab;
	public GameObject FinishPrefab;
	public GameObject GroundPrefab;
	public GameObject CollectiblePrefab;

	private Level level;

	public void Initialize (Level level) {
		setLevel(level);
		SpawnLevel();
	}
	
	public void SpawnLevel () {
		destroyCurrentLevel();

		LevelPiece[,] pieces = this.level.GetLevelPieces();

		for (int x = 0; x < this.level.GetWidth(); x++) {
			for (int y = 0; y < this.level.GetHeight(); y++) {
				spawnPrefab(pieces[x,y], x, y);
			}
		}
	}

	private void destroyCurrentLevel () {
		for (int i = 0; i < transform.childCount; i++) {
			Debug.Log(transform.GetChild(i).gameObject);
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	private void setLevel (Level level) {
		this.level = level;
	}

	private void spawnPrefab (LevelPiece type, int x, int y) {
		GameObject prefab = getPrefab(type);

		if (prefab == null) {
			return;
		}

		GameObject piece = (GameObject) Instantiate(
			prefab,
			LevelPositioner.PositionPiece(x, y),
			Quaternion.identity
		);

		setParent(piece);

		if (piece == GroundPrefab) {
			LevelController.Instance.GroundHeight = piece.transform.position.y;
		}

	}

	private void setParent (GameObject prefab) {
		prefab.transform.parent = transform;
	}

	private GameObject getPrefab (LevelPiece type) {
		switch (type) {
		case LevelPiece.Collectible:
			return CollectiblePrefab;
		case LevelPiece.Player:
			return PlayerPrefab;
		case LevelPiece.Finish:
			return FinishPrefab;
		case LevelPiece.Ground:
			return GroundPrefab;
		case LevelPiece.Platform:
			return PlatformPrefab;
		default:
			return null;
		}
	}
}
