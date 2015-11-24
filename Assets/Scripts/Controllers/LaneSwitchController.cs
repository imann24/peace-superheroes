using UnityEngine;
using System;
using System.Collections;

public class LaneSwitchController : MonoBehaviour {
	int currentLane = 1;

	void Start () {
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	void Update () {
		if (MovementController.Instance.Paused) {
			return;
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			changeLane(Direction.Up);
		} else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			changeLane(Direction.Down);
		}
	}

	void changeLane (Direction direction) {

		switch (direction) {
			case Direction.Up:
				if (laneInBounds(currentLane + 1)) {
					currentLane++;
				}
				break;
			case Direction.Down:
				if (laneInBounds(currentLane - 1)) {
					currentLane--;
				}
				break;
			default:
				break;
		}
		setPlayerPosition();
	}


	void setPlayerPosition (float yOffset = -0.135f) {
		Vector3 lanePoint = SpawnPointController.GetSpawnPosition((SpawnPoint) currentLane);
		lanePoint.y += yOffset;
		lanePoint.z = transform.position.z;
		lanePoint.x = transform.position.x;
		transform.position = lanePoint;
	}

	bool laneInBounds (int targetLane) {
		return (targetLane >= 0 &&
		        targetLane < Enum.GetNames(typeof(SpawnPoint)).Length);
	}

	void subscribeEvents () {
		SwipeController.OnSwipe += changeLane;
	} 

	void unsubscribeEvents () {
		SwipeController.OnSwipe -= changeLane;
	}
}
