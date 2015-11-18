using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPointController : MonoBehaviour {

	private static Dictionary<SpawnPoint, SpawnPointController> All = 
		new Dictionary<SpawnPoint, SpawnPointController>();

	public SpawnPoint spawnPoint;

	void Awake () {
		initialize();
	}

	void initialize () {
		if (All.ContainsKey(spawnPoint)) {
			Debug.LogWarning("Dictionary already contains this type");
		} else {
			All.Add(spawnPoint, this);
		}
	}

	void OnDestroy () {
		All.Clear();
	}

	Vector3 getPositon () {
		return transform.position;
	}

	public static Vector3 GetSpawnPosition (SpawnPoint spawnPoint) {
		if (All.ContainsKey(spawnPoint)) {
			return All[spawnPoint].getPositon();
		} else {
			return Global.ZERO_VECTOR;
		}
	}
}
