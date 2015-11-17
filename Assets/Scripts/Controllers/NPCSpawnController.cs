using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class NPCSpawnController : MonoBehaviour {
	public delegate void NPCEncounterAction (Emotion emotion);
	public static event NPCEncounterAction OnNPCEncounter;

	public GameObject NPCPrefab;
	public float SpawnFrequency = 5.0f;

	private float timeToAct;

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
	}
	
	// Update is called once per frame
	void Update () {
		if (!Global.Paused) {
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

			if (spawnPools[i].Count > 0) {
				npc = spawnNPCFromPool(spawnPoint).GetComponent<NPCController>();
				setNPCStartPosition(
					npc, 
					spawnPoint
					);
			} else {
				npc = spawnNPC (spawnPoint).GetComponent<NPCController>();
			}

			npc.Emotion = generateEmotion();
		}
	}

	void initialize () {
		timeToAct = SpawnFrequency;
		for (int i = 0; i < spawnPools.Length; i++) {
			spawnPools[i] = new Queue<GameObject>();
		}
	}

	void subscribeToNPCEvents (NPCController npc) {
		npc.OnCollidedWithPlayer += callNPCEncounterEvent;
		npc.OnOffscreen += handleNPCOffscreen;
	}

	void spawnOnTimer () {
		if (Time.time > timeToAct) {
			spawnColumnOfNPCs();
			timeToAct += SpawnFrequency;
		}
	}

	void handleNPCOffscreen (GameObject npc, SpawnPoint spawnPoint) {
		spawnPools[(int) spawnPoint].Enqueue(npc);
	}

	void callNPCEncounterEvent (Emotion emotion) {
		if (OnNPCEncounter != null) {
			OnNPCEncounter(emotion);
		}
	}

	Emotion generateEmotion () {
		return (Emotion) (UnityEngine.Random.Range(
			0, Enum.GetNames(typeof(Emotion)).Length + 1) % 
			Enum.GetNames(typeof(Emotion)).Length);
	}
}
