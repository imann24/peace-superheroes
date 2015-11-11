using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelPieceController : MonoBehaviour {
	public delegate void CollectAction(string collectible);
	public static event CollectAction OnCollect;
	public LevelPiece Type;

	private static List<GameObject> collected = new List<GameObject>();

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

		collected.Add (gameObject);
		gameObject.SetActive(false);
	}

	public static void ReactivateAllCollectibles () {
		for (int i = 0; i < collected.Count; i++) {
			collected[i].SetActive(true);
		}

		collected.Clear ();
	}
}
