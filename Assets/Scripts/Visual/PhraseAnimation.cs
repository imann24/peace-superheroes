using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseAnimation : MonoBehaviour {
	public delegate void PhraseCollectedAction();
	public static event PhraseCollectedAction OnPhraseCollected;
	public float Offset = 10;
	private Vector2 defaultPosition;
	private Vector2 defaultScale;
	private Vector2 offsetPosition;
	private Image image;
	// Use this for initialization
	void Start () {
		setReferences();
		sendOffscreen();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void StartAnimation (Direction direction) {
		resetPositionAndScale();
		StartCoroutine(LerpAndShrink(direction));

	}

	private void resetPositionAndScale () {
		image.enabled = true;
		transform.position = defaultPosition;
		transform.localScale = defaultScale;
	}

	private void setReferences () {
		image = GetComponent<Image>();
		defaultPosition = transform.position;
		defaultScale = transform.localScale;
		offsetPosition = new Vector2 (Offset, Offset);
	}

	private void sendOffscreen () {
		transform.position = new Vector2 (0, Screen.height * 2);
	}

	private void callOnPhraseCollectedEvent () {
		if (OnPhraseCollected != null) {
			OnPhraseCollected();
		}
	}

	IEnumerator LerpAndShrink (Direction direction, float time = Global.DONE_TIME, float speed = 1.25f) {
		Vector2 targetPosition;
		Vector2 startPosition = transform.position;

		Vector2 startScale = transform.localScale;
		Vector2 targetScale = new Vector2(0, 0);

		float timer = 0;

		switch (direction) {
			case Direction.Up:
				targetPosition = new Vector2(Screen.width, Screen.height) + offsetPosition;
				break;
			case Direction.Down:
				targetPosition = new Vector2(0, 0) - offsetPosition;
				break;
			default:
				targetPosition = new Vector2(0, 0);
				break;
		}

		while (timer < time) {
			transform.position = Vector2.Lerp(startPosition, 
			                                  targetPosition,
			                                  timer);


			transform.localScale = Vector2.Lerp(startScale,
			                                    targetScale,
			                                    timer);
			timer += Time.deltaTime * speed;

			yield return new WaitForEndOfFrame();
		}

		if (direction == Direction.Up) {
			callOnPhraseCollectedEvent();
		}

		image.enabled = false;

	}
}
