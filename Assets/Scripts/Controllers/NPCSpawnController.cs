using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class NPCSpawnController : MonoBehaviour {
	public delegate void NPCEncounterAction (Emotion emotion, string phrase);
	public static event NPCEncounterAction OnNPCEncounter;

	public GameObject NPCPrefab;

	public float SpawnFrequency = 5.0f;
	public float MaxSpawnFrequency = 3.0f;
	public float SpawnFrequencyIncrease = 0.1f;
	private float timeToAct;
	private float timer;
	private int angryNPCSSpawnCountSinceMentorNPC = 3;
	private int maxAngryNPCSpawnConsecutively = 3;

	private Emotion[] spawnableEmotions = {
		Emotion.Mad,
		Emotion.None
	};

	private int [] spawnWeights = {
		2,
		1
	};

	private Emotion[] emotionSelection;

	private SpawnPoint [] spawnPoints = {
		SpawnPoint.HighPlatform,
		SpawnPoint.MidPlatform,
		SpawnPoint.LowPlatform
	};

	private Queue<GameObject> [] spawnPools = 
		new Queue<GameObject> [Enum.GetNames(typeof(SpawnPoint)).Length];
	
	// Use this for initialization
	void Start () {
		initialize();
		initializeEmotionSelection();
	}
	
	// Update is called once per frame
	void Update () {
		if (!MovementController.Instance.Paused) {
			spawnOnTimer();
		}
	}

	GameObject spawnNPC (SpawnPoint spawnPoint) {
		GameObject npc = (GameObject) Instantiate (
			NPCPrefab,
			SpawnPointController.GetSpawnPosition(spawnPoint),
			Quaternion.identity);

		setNPCReferences(
			npc.GetComponent<NPCController>(),
			spawnPoint);

		return npc;
	}

	void setNPCReferences (NPCController npc, SpawnPoint spawnPoint) {
		npc.SetSpawnPoint(spawnPoint);
		subscribeToNPCEvents(npc);
	}

	void setNPCStartPosition (NPCController npc, SpawnPoint spawnPoint) {
		npc.transform.position = 
			SpawnPointController.GetSpawnPosition(spawnPoint);

		npc.moveAcrossScreenToLeft();
	}

	GameObject spawnNPCFromPool (SpawnPoint spawnPoint) {
		Queue<GameObject> spawnPool = spawnPools[(int) spawnPoint];
		if (spawnPool.Count > 0) {
			return spawnPool.Dequeue();
		} else {
			return spawnNPC(spawnPoint);
		}
	}

	void spawnColumnOfNPCs () {
		for (int i = 0; i < spawnPools.Length; i++) {
			SpawnPoint spawnPoint = (SpawnPoint) i;
			NPCController npc;
			GameObject npcObject;

			if (spawnPools[i].Count > 0) {
				npcObject = spawnNPCFromPool(spawnPoint);
				npc = npcObject.GetComponent<NPCController>();
				setNPCStartPosition(
					npc, 
					spawnPoint
					);
			} else {
				npcObject = spawnNPC (spawnPoint);
				npc = npcObject.GetComponent<NPCController>();
			}

			npc.Emotion = generateEmotion();
			if (npc.Emotion == Emotion.None) {
				DisplayPhrases.Instance.SpawnPhrase(npcObject, PhraseController.Instance.GetRandomPhrase(), 1.0f);
			} else if (npc.Emotion == Emotion.Mad) {
				npc.SetConflictPhrase(PhraseController.Instance.GetRandomConflictPhrase());
			}
		}
	}

	void initialize () {
		timeToAct = SpawnFrequency;
		for (int i = 0; i < spawnPools.Length; i++) {
			spawnPools[i] = new Queue<GameObject>();
		}
		MovementController.Instance.Paused = false;
	}

	void subscribeToNPCEvents (NPCController npc) {
		npc.OnCollidedWithPlayer += callNPCEncounterEvent;
		npc.OnOffscreen += handleNPCOffscreen;
	}

	void spawnOnTimer () {
		timer+= Time.deltaTime;

		if (timer > timeToAct) {
			spawnColumnOfNPCs();
			SpawnFrequency = Mathf.Clamp(SpawnFrequency - SpawnFrequencyIncrease, 
			                             MaxSpawnFrequency, 
			                             float.MaxValue);
			timeToAct += SpawnFrequency;
		}
	}

	void handleNPCOffscreen (GameObject npc, SpawnPoint spawnPoint) {
		spawnPools[(int) spawnPoint].Enqueue(npc);
	}

	void callNPCEncounterEvent (Emotion emotion, string phrase) {
		if (OnNPCEncounter != null) {
			OnNPCEncounter(emotion, phrase);
		}
	}

	Emotion generateEmotion () {
		Emotion emotion = emotionSelection[UnityEngine.Random.Range(0, emotionSelection.Length)];

		if (emotion == Emotion.Mad) {
			angryNPCSSpawnCountSinceMentorNPC++;
		}

		if (angryNPCSSpawnCountSinceMentorNPC > maxAngryNPCSpawnConsecutively) {
			emotion = Emotion.None;
			angryNPCSSpawnCountSinceMentorNPC = 0;
		}

		return emotion;
	}

	private void initializeEmotionSelection () {
		int length = 0;

		for (int i = 0; i < spawnWeights.Length; i++) {
			length += spawnWeights[i];
		}

		emotionSelection = new Emotion[length];

		int index = 0;

		for (int i = 0; i < spawnWeights.Length; i++) {
			for (int j = 0; j < spawnWeights[i]; j++) {
				emotionSelection[index++] = spawnableEmotions[i];
			}
		}
	}
}
