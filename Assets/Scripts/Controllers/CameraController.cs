using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	IEnumerator yLerpCoroutine;
	Transform player;

	// Use this for initialization
	void Start () {
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
		if (player == null) {
			return;
		}
		transform.position = Util.MatchPosition(
			player,
			transform,
			true, false, false);
		if (!inRangeOfPlayer(player.position)) {
			lerpCamera(player.position);
		}
	}

	public void SetPlayer (Transform player) {
		this.player = player;
	}

	private void SetPlayer () {
		SetPlayer(PlayerController.Instance.GetTransform ());
	}

	IEnumerator lerpToPlayerPosition (Vector3 playerPosition, float steps = 15, float tolerance = 0.05f) {
		float yStep = yDistance(playerPosition)/steps;
	
		while (!inRangeOfPlayer(playerPosition)) {
			transform.position = yTranslate(transform.position, yStep);
			yield return new WaitForEndOfFrame();
		}

		if (yLerpCoroutine != null) {
			StopCoroutine(yLerpCoroutine);
			yLerpCoroutine = null;
		}
	}

	bool inRangeOfPlayer (Vector3 playerPosition, float tolerance = 0.05f) {
		return Mathf.Abs(yDistance(playerPosition)) < tolerance;
	}

	float yDistance (Vector3 objectPosition) {
		return objectPosition.y - transform.position.y;
	}

	void lerpCamera (Vector3 playerPosition) {

		if (yLerpCoroutine != null) {
			return;
		}

		yLerpCoroutine = lerpToPlayerPosition(playerPosition);

		StartCoroutine(yLerpCoroutine);
	}

	Vector3 yTranslate (Vector3 position, float deltaY) {
		return new Vector3(position.x,
		                   position.y + deltaY,
		                   position.z);
	}

	void subscribeEvents () {
		PlayerController.OnPlayerSet += SetPlayer;
	}

	void unsubscribeEvents () {
		PlayerController.OnPlayerSet -= SetPlayer;
	}

}
