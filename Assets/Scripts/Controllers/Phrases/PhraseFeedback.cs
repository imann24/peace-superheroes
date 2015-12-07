using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseFeedback : MonoBehaviour {
	public static PhraseFeedback Instance;
	public Text Feedback;

	void Awake () {
		Instance = this;
	}

	void OnDestroy () {
		Instance = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ActivateFeedback (string feedback) {
		gameObject.SetActive(true);
		Feedback.text = feedback;
	}

	public void DeactivateFeedback () {
		gameObject.SetActive(false);
	}
}
