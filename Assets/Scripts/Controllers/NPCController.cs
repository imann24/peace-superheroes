using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class NPCController : MonoBehaviour {
	private static Color calmColor = Color.green;
	private static Color madColor = Color.red;

	public delegate void OffscreenAction(GameObject g, SpawnPoint spawnPoint);
	public event OffscreenAction OnOffscreen;

	public delegate void CollidedWithPlayerAction(Emotion emotion);
	public event CollidedWithPlayerAction OnCollidedWithPlayer;

	private SpriteRenderer spriteRenderer;

	IEnumerator moveCoroutine;
	IEnumerator colorCoroutine;

	private SpawnPoint spawnPoint;

	private Emotion _emotion;

	private bool _onScreen;

	public bool OnScreen {
		get {
			return _onScreen;
		}

		private set {
			_onScreen = value;
			if (!value) {
				callOffscreenEvent();
			}
		}
	}

	public Emotion Emotion {
		get {
			return _emotion;
		}

		set {
			_emotion = value;
			setVisualEmotion();
		}
	}

	void Awake () {
		setReferences();
	}

	// Use this for initialization
	void Start () {
		moveAcrossScreenToLeft();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Moves the character to the right of the screen
	public void moveAcrossScreenToLeft () {
		if (moveCoroutine != null) {
			StopCoroutine(moveCoroutine);
		}

		moveCoroutine = lerpPosition(findTargetPoint(), 0.5f);

		StartCoroutine(moveCoroutine);
	}

	public void transitionToColor (Color color) {
		if (colorCoroutine != null) {
			StopCoroutine(colorCoroutine);
		}

		colorCoroutine = lerpColor(color);

		StartCoroutine(colorCoroutine);
	}

	void OnTriggerEnter2D (Collider2D collider) {

		switch (collider.gameObject.tag) {
			case Tags.OFFSCREEN_TAG:
				OnScreen = false;
				break;
			case Tags.PLAYER:
				callCollidedWithPlayerEvent();
				break;
			default:
				break;
		}


	}

	void initialize () {
		OnScreen = true;
	}

	void callOffscreenEvent () {
		if (OnOffscreen != null) {
			OnOffscreen(gameObject, spawnPoint);
		}
	}

	void callCollidedWithPlayerEvent () {
		if (OnCollidedWithPlayer != null) {
			OnCollidedWithPlayer(_emotion);
		}
	}

	void setVisualEmotion () {
		if (_emotion == Emotion.Calm) {
			transitionToColor(calmColor);
		} else if (_emotion == Emotion.Mad) {
			transitionToColor(madColor);
		}
	}

	void setReferences () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	Vector3 findTargetPoint (float xOffset = -1.0f) {
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
		targetPoint.y = transform.position.y;
		targetPoint.x += xOffset;
		return targetPoint;
	}

	// Lerps to the specified position
	IEnumerator lerpPosition (Vector3 endPosition, float speed = Global.DONE_TIME) {
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

	}

	// Lerps to the specified color
	IEnumerator lerpColor (Color endCollor, float speed = Global.DONE_TIME) {
		Color startColor = spriteRenderer.color;
		float time = 0;
		
		while (time < Global.DONE_TIME) {
			spriteRenderer.color = 
				Color.Lerp(
					startColor,
					endCollor,
					time
					);
			time += Time.deltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		
	}


	public void SetSpawnPoint (SpawnPoint spawnPoint) {
		this.spawnPoint = spawnPoint;
	}

}
