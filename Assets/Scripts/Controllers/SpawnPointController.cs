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

	public static Vector3 GetSpawnPosition (SpawnPoint spawnPoint, bool addOffset = false, float yOffset = -0.010f) {
		if (All.ContainsKey(spawnPoint)) {
			Vector3 position = All[spawnPoint].getPositon();
			if (addOffset) {
				position.y += yOffset;
			}
			return position;
		} else {
			return Global.ZERO_VECTOR;
		}
	}
}
