using UnityEngine;
using System.Collections;

public class OrbCollectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		collect();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void collect () {
		StartCoroutine(
			lerpPosition(
			findTargetPoint()));
	}

	Vector3 findTargetPoint (float xOffset = -5.0f) {
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
		return targetPoint;
	}
	
	// Lerps to the specified position
	IEnumerator lerpPosition (Vector3 endPosition, float speed = Global.DONE_TIME, bool destroyWhenComplete = true) {
		Vector3 startPosition = transform.position;
		float time = 0;
		
		while (time < Global.DONE_TIME) {
			transform.position = 
				Vector3.Lerp(
					startPosition,
					endPosition,
					time
					);
			time += Time.deltaTime * speed;
			yield return new WaitForEndOfFrame();
		}

		if (destroyWhenComplete) {
			Destroy(gameObject);
		}
		
	}
}
