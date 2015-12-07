using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]

public class FlashEffect : MonoBehaviour {
	public bool running = true;
	CanvasGroup canvasGroup;

	private bool increasing = false;

	// Use this for initialization
	void Start () {
		setReferences();
		startFlashEffect();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setReferences () {
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public void startFlashEffect () {
		StartCoroutine(flashEffect());
	}

	public void endFlashEffect () {
		running = false;
	}

	IEnumerator flashEffect (float speed = 1.0f, float transparent = 0.25f, float opaque = 0.75f) {
		while (running) {
			float timer = 0;
			float startOpacity = increasing ? transparent : opaque;
			float endOpacity = increasing ? opaque : transparent;

			while (timer <= 1.0) {
				canvasGroup.alpha = Mathf.Lerp(
					startOpacity,
					endOpacity,
					timer);

				timer += Time.deltaTime * speed;

				yield return new WaitForEndOfFrame();
			}

			increasing = !increasing;
		}
	}
}
