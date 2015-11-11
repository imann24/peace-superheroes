using UnityEngine;
using System.Collections;

public class LevelPieceController : MonoBehaviour {
	public delegate void CollectAction(string collectible);
	public static event CollectAction OnCollect;
	public LevelPiece Type;

	private string collectible = PhraseController.DEFAULT_PHRASE;

	void OnTriggerEnter2D (Collider2D collider) {
		if (Type == LevelPiece.Collectible) {
			callOnCollect();
		}
	}

	void callOnCollect () {
		if (OnCollect != null) {
			OnCollect(collectible);
		}

		Destroy(gameObject);
	}
}
