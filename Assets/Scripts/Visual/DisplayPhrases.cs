using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayPhrases : MonoBehaviour {
	public static DisplayPhrases Instance;

	public GameObject PhrasePrefab;
	public GameObject Player;
	public GameObject PhraseApprover;
	public GameObject PhraseSelector;

	private Vector2 phraseOffset = new Vector2 (250, -25);
	private Vector2 offscreenSpawnPoint = new Vector2 (-100, -100);
	private Queue<GameObject> spawnPool = new Queue<GameObject>();

	void Awake () {
		Instance = this;
		subscribeEvents();
	}

	// Use this for initialization
	void Start () {
	}


	void OnDestroy () {
		unsubscribeEvents();
		Instance = this;
	}

	// Update is called once per frame
	void Update () {

	}

	public void DisplayPhraseApprover (string phrase) {
		PhraseApprover.SetActive(true);
		PhraseApprover.GetComponent<PhraseApprover>().SetPhrase(phrase);
	}

	public void DisplayPhraseSelector (string conflictPhrase = "This is a conflict we're having") {
		PhraseSelector.SetActive(true);

		PhraseSelector controller = this.PhraseSelector.GetComponent<PhraseSelector>();
		controller.SetConflictPhrase(conflictPhrase);
		controller.SpawnPhrases(PhraseCollector.Instance.GetAllCollectedPhrases());
	}

	public void SpawnPhrase (GameObject objecToTrack, string phraseText, float scale = 0.25f) {
		GameObject phrase;
		bool newlySpawned = false;
		if (spawnPool.Count == 0) {
			phrase = (GameObject) Instantiate(PhrasePrefab,
			                                  offscreenSpawnPoint,
			                                  Quaternion.identity);
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

		NPCController npc = objecToTrack.GetComponent<NPCController>();

		if (npc == null) {
			Debug.LogError("NPCController not attached to NPC");
			return;
		}

		npc.Phrase = phraseText;
	}

	void handlePhraseNotNeeded (GameObject uneededPhrase) {
		spawnPool.Enqueue(uneededPhrase);
	}

	void handleNPCEncounter (Emotion npcEmotion, string phrase) {
		if (npcEmotion == Emotion.None) {
			if (PhraseValidator.CanAcceptPhrase(phrase)) {
				DisplayPhraseApprover(phrase);
				MovementController.Instance.Paused = true;
			} else {
				NotificationController.Instance.ShowNotification(
					Notification.MaxPhrases);
			}
		} else if (npcEmotion == Emotion.Mad) {
		    if (TrackerController.Instance.PhraseCount() > 0){
				DisplayPhraseSelector(phrase);
				MovementController.Instance.Paused = true;
			} else {
				TrackerController.Instance.AngryEncounterWithoutPhrase();
			}
		}
	}

	void subscribeEvents () {
		NPCSpawnController.OnNPCEncounter += handleNPCEncounter;
	}

	void unsubscribeEvents () {
		NPCSpawnController.OnNPCEncounter -= handleNPCEncounter;
	}

}
