using UnityEngine;
using System;
using System.Collections;

public class LaneSwitchController : MonoBehaviour {
	public delegate void SwitchToLaneAction (int lane);
	public static event SwitchToLaneAction OnSwitchToLane;

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
					callOnSwitchToLane();
				}
				break;
			case Direction.Down:
				if (laneInBounds(currentLane - 1)) {
					currentLane--;
					callOnSwitchToLane();
				}
				break;
			default:
				break;
		}
		setPlayerPosition();
	}


	void setPlayerPosition (float yOffset = -0.150f) {
		Vector3 lanePoint = SpawnPointController.GetSpawnPosition((SpawnPoint) currentLane);
		lanePoint.y += yOffset;
		lanePoint.z = transform.position.z;
		lanePoint.x = transform.position.x;
		transform.position = lanePoint;
	}

	bool laneInBounds (int targetLane) {
		return (targetLane >= 0 &&
		        targetLane < LaneCount());
	}

	void subscribeEvents () {
		SwipeController.OnSwipe += changeLane;
	} 

	void unsubscribeEvents () {
		SwipeController.OnSwipe -= changeLane;
	}

	void callOnSwitchToLane () {
		if (OnSwitchToLane != null) {
			OnSwitchToLane(currentLane);
		}
	}

	public static int LaneCount () {
		return Enum.GetNames(typeof(SpawnPoint)).Length;
	}
}
