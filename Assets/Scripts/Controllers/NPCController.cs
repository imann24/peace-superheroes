using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class NPCController : MonoBehaviour {
	public float Speed = 1.0f;

	public delegate void OffscreenAction(GameObject g, SpawnPoint spawnPoint);
	public OffscreenAction OnOffscreen;

	public delegate void CollidedWithPlayerAction(Emotion emotion, string phrase);
	public event CollidedWithPlayerAction OnCollidedWithPlayer;

	public GameObject PhrasePrefab;

	private SpriteRenderer spriteRenderer;

	IEnumerator moveCoroutine;
	IEnumerator colorCoroutine;

	private SpawnPoint spawnPoint;

	private Emotion _emotion;

	private bool _onScreen;

	private GameObject currentPhrase;

	private string _phrase;

	public string Phrase {
		get {
			return _phrase;
		}

		set {
			_phrase = value;
		}
	}

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
			if (value == Emotion.Mad) {
				destroyCurrentPhrase();
			}

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

		moveCoroutine = lerpPosition(findTargetPoint(), Speed);

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
				//OnScreen = false;
				break;
			case Tags.PLAYER:
				bool calmedDown = false;
				if (Emotion == Emotion.Mad &&
			    	PhraseCollector.Instance.GetPhraseCount() > 0) {
					calmedDown = true;
				} 
				callCollidedWithPlayerEvent();
				if (calmedDown) {
					Emotion = Emotion.Calm;
				} else if (Emotion == Emotion.Mad) {
					Emotion = Emotion.VeryMad;
				}

				setVisualEmotion();
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
			OnCollidedWithPlayer(_emotion, _phrase);
		}
	}

	void setVisualEmotion () {
		if (_emotion == Emotion.Calm) {
			spriteRenderer.sprite = SpriteHolder.GetSprite(Sprites.CalmNPC);
		} else if (_emotion == Emotion.Mad) {
			spriteRenderer.sprite = SpriteHolder.GetSprite(Sprites.AngryNPC);
		} else if (_emotion == Emotion.VeryMad) {
			spriteRenderer.sprite = SpriteHolder.GetSprite(Sprites.AngrierNPC);
		} else {
			spriteRenderer.sprite = SpriteHolder.GetSprite(Sprites.MentorNPC);
		}
	}

	void setReferences () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	Vector3 findTargetPoint (float xOffset = -5.0f) {
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
			if (MovementController.Instance.Paused) {
				yield return new WaitForEndOfFrame();
				continue;
			}

			transform.position = 
				Vector3.Lerp(
					startPosition,
					endPosition,
					time
				);
			time += Time.deltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		OnScreen = false;
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

	public void SetConflictPhrase (string phrase) {
		Phrase = phrase;
	}

	public void SpawnPhrase (float xOffset = 0.25f) {
		destroyCurrentPhrase();
		GameObject phrase = (GameObject)
			Instantiate (
			PhrasePrefab,
			transform.position,
			Quaternion.identity);
		phrase.GetComponent<Phrase>().SetPhrase(
			PhraseController.Instance.GetRandomPhrase());

		phrase.transform.Translate(Vector3.right * xOffset);
		phrase.transform.parent = transform;

		currentPhrase = phrase;
	}

	private void destroyCurrentPhrase () {
		if (currentPhrase != null) {
			Destroy(currentPhrase);
		}
	}

}
