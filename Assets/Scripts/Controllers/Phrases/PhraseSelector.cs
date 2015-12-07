using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseSelector : MonoBehaviour {
	public static event PhraseApprover.PhraseChoiceAction OnPhraseChoice;

	public static PhraseSelector Instance;

	public GameObject StoredPhrasePrefab;
	public GameObject FeedbackCanvas;
	public Transform PhraseHolder;
	public Text ConflictPhrase;
	public float cellHeight = 200;
	RectTransform rectTransform;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		setReferences();
	}

	void OnDestroy () {
		Instance = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CloseSelector () {
		callPhraseChosenEvent(true);
		gameObject.SetActive(false);
		MovementController.Instance.Paused = false;
		activateFeedback("This is the feedback you're getting");
	}

	public void SetConflictPhrase (string phrase) {
		ConflictPhrase.text = phrase;
	}

	public void SpawnPhrases (string [] phrases) {
		Util.DeleteAllChildren(PhraseHolder);
		for (int i = 0; i < phrases.Length; i++) {
			setPhrase(SpawnPhrase(), phrases[i]);
		}
		PhraseHolder.GetComponent<RectTransform>().sizeDelta = new Vector2 (
			PhraseHolder.GetComponent<RectTransform>().sizeDelta.x,
			cellHeight * phrases.Length);
	}

	private void activateFeedback (string feedback) {
		FeedbackCanvas.SetActive(true);
		PhraseFeedback.Instance.ActivateFeedback(feedback);
	}

	private GameObject SpawnPhrase () {
		GameObject phrase = (GameObject) Instantiate(StoredPhrasePrefab);

		phrase.transform.SetParent(PhraseHolder);

		return phrase;
	}
	
	private void setPhrase (GameObject phraseObject, string text) {
		StoredPhraseController phrase = phraseObject.GetComponent<StoredPhraseController>();

		if (phrase == null) {
			Debug.LogError("Controller is null on the phrase");
			return;
		}

		phrase.SetText(text);
	}

	private void setReferences () {
		rectTransform = GetComponent<RectTransform>();
	}

	private void callPhraseChosenEvent (bool phraseApproved) {
		if (OnPhraseChoice != null) {
			OnPhraseChoice(phraseApproved);
		}
	}

}
