using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NotificationController : MonoBehaviour {
	public static NotificationController Instance;

	public Text UINotification;

	public float NotificationSpeed = 1.5f;
	public float NotificationLifeTime = 3.0f;
	public float YOffset = 75;
	public float XOffset;

	//------------------------------------------
	//Phrases

	public string MaxPhrasesMessage = "You cannot hold anymore phrases";

	//------------------------------------------

	private string hideNotificationFunction = "moveNotificationOffscreen";
	private CanvasGroup canvasGroup;
	private Vector3 onScreenPosition;
	private Vector3 offScreenPosition;
	private RectTransform rectTransform;
	
	IEnumerator moveCoroutine;
	IEnumerator transparencyCoroutine;

	void Awake () {
		Instance = this;
		setReferences();
		moveNotificationOffscreen();
		subscribeEvents();
	}

	void OnDestroy () {
		Instance = null;
		unsubscribeEvents();
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			moveNotificationOnscreen();
		}
	}

	public void DismissNotification () {
		moveNotificationOffscreen();
	}

	public void ShowNotification (string notification) {
		setNotification(notification);
		moveNotificationOnscreen();
	}

	public void ShowNotification (Notification notification) {
		switch (notification) {
		case Notification.MaxPhrases:
			ShowNotification(MaxPhrasesMessage);
			break;
		default:
			Debug.LogWarning("No message implemented for " + notification);
			break;
		}
	}

	void setNotification (string message) {
		UINotification.text = message;
	}

	void setReferences () {
		canvasGroup = GetComponent<CanvasGroup>();
		rectTransform = GetComponent<RectTransform>();
		onScreenPosition = new Vector3(
			Screen.width/2 + XOffset,
			Screen.height - YOffset);
		offScreenPosition = 
			new Vector3(
				Screen.width/2 + XOffset,
				Screen.height + YOffset);
	}

	void moveNotificationOnscreen (bool temporary = true) {
		if (temporary) {
			Invoke(
				hideNotificationFunction,
				NotificationLifeTime);
		}

		startMovementCoroutine(onScreenPosition);
		startTransparencyCoroutine(1.0f);
	}

	void moveNotificationOffscreen () {
		startMovementCoroutine(offScreenPosition);
		startTransparencyCoroutine(0.0f);
	}

	void startMovementCoroutine (Vector3 targetPosition, float speed = 1f) {
		stopCoroutine(moveCoroutine);
		moveCoroutine = lerpPosition(targetPosition, NotificationSpeed);
		StartCoroutine(moveCoroutine);
	}

	void startTransparencyCoroutine (float targetTransparency, float speed = 1f) {
		stopCoroutine(transparencyCoroutine);
		transparencyCoroutine = lerpTransparency(targetTransparency, NotificationSpeed);
		StartCoroutine(transparencyCoroutine);
	}

	void stopCoroutine (IEnumerator coroutine) {
		if (coroutine != null) {
			StopCoroutine(coroutine);
		}
	}

	void subscribeEvents () {

	}

	void unsubscribeEvents () {
		
	}

	IEnumerator lerpPosition (Vector3 targetPosition, float speed = 1f) {
		float timer = 0;
		Vector3 startPosition = transform.position;

		while (timer <= 1) {
			timer += Time.deltaTime * speed;

			transform.position = Vector3.Lerp(
				startPosition,
				targetPosition,
				timer);

			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator lerpTransparency (float targetTransparency, float speed = 1f) {
		float timer = 0;
		float startTransparency = canvasGroup.alpha;

		while (timer <= 1) {
			timer += Time.deltaTime * speed;
			
			canvasGroup.alpha = Mathf.Lerp(
				startTransparency,
				targetTransparency,
				timer);
			
			yield return new WaitForEndOfFrame();
		}
	}
}
