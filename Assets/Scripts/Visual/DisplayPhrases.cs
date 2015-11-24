using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayPhrases : MonoBehaviour {
	public static DisplayPhrases Instance;

	public GameObject PhrasePrefab;
	public GameObject Player;
	public GameObject PhraseApprover;

	private Vector2 phraseOffset = new Vector2 (250, -25);
	private Queue<GameObject> spawnPool = new Queue<GameObject>();

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	}


	void OnDestroy () {
		Instance = this;
	}

	// Update is called once per frame
	void Update () {

	}

	public void DisplayPhraseApprover () {
		PhraseApprover.SetActive(true);
	}

	public void SpawnPhrase (GameObject objecToTrack, string phraseText, float scale = 0.25f) {
		GameObject phrase;
		bool newlySpawned = false;
		if (spawnPool.Count == 0) {
			phrase = (GameObject) Instantiate(PhrasePrefab);
			phrase.transform.SetParent(transform);
			phrase.transform.localScale *= scale;
			newlySpawned = true;
		} else {
			phrase = spawnPool.Dequeue();
		}

		TrackUIWithGameObject tracker = phrase.GetComponent<TrackUIWithGameObject>();
		if (tracker == null) {
			Debug.LogError ("Tracker script not attached");
			return;
		}

		if (newlySpawned) {
			tracker.OnTrackingFinished += handlePhraseNotNeeded;
		}

		tracker.SetParentRect(gameObject);
		tracker.SetObjecToTrack(objecToTrack);
		tracker.SetOffset(phraseOffset);

		Phrase phraseController = tracker.GetComponent<Phrase>();

		if (phraseController == null) {
			Debug.LogError ("Phrase script not attached");
			return;
		}

		phraseController.SetPhrase(phraseText);
	}

	void handlePhraseNotNeeded (GameObject uneededPhrase) {
		spawnPool.Enqueue(uneededPhrase);
	}
}
