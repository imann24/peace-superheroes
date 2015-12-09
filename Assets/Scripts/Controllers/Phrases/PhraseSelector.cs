using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseSelector : MonoBehaviour {
	public static event PhraseApprover.PhraseChoiceAction OnPhraseChoice;

	public static PhraseSelector Instance;

	public GameObject StoredPhrasePrefab;
	public GameObject FeedbackCanvas;
	public ScrollRect scrollRect;
	public Transform PhraseHolder;
	public Text ConflictPhrase;
	public float cellHeight = 200;
	RectTransform rectTransform;
	private int phraseCount;
	private NPCController npc;

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

	void OnEnable () {
		ResetScrollRectPosition();
	}

	public void UsePhrase (string response) {
		Quality quality = PhraseController.Instance.ScorePhrase(response, ConflictPhrase.text);
		callPhraseChosenEvent(quality);
		string feedback = PhraseValidator.GetFeedback(quality);

		if (npc != null) {
			if (quality == Quality.Bad) {
				npc.Emotion = Emotion.VeryMad;
			} else {
				npc.Emotion = Emotion.Calm;
			}
		}
		CloseSelector(feedback, quality);
	}


	public void CloseSelector (string feedback, Quality quality) {
		gameObject.SetActive(false);
		activateFeedback(feedback, quality);
	}

	public void SetConflictPhrase (string phrase) {
		ConflictPhrase.text = phrase;
	}

	public string GetConflictPhrase () {
		return ConflictPhrase.text;
	}

	public void SpawnPhrases (string [] phrases) {
		Util.DeleteAllChildren(PhraseHolder);
		for (int i = 0; i < phrases.Length; i++) {
			setPhrase(SpawnPhrase(), phrases[i]);
		}

		phraseCount = phrases.Length;
		ScaleScrollRect();
	}

	public void ScaleScrollRect () {
		PhraseHolder.GetComponent<RectTransform>().sizeDelta = new Vector2 (
			PhraseHolder.GetComponent<RectTransform>().sizeDelta.x,
			(cellHeight * phraseCount)/3.0f);
	}

	// http://answers.unity3d.com/questions/801380/force-scrollbar-to-scroll-down-with-scrollrect.html
	public void ResetScrollRectPosition () {
		Canvas.ForceUpdateCanvases();
		scrollRect.velocity = new Vector2 (0, -1000);
		Canvas.ForceUpdateCanvases();
	}

	public void SetNPC (NPCController npc) {
		this.npc = npc;
	}

	private void activateFeedback (string feedback, Quality qualityOfResponse) {
		FeedbackCanvas.SetActive(true);
		PhraseFeedback.Instance.ActivateFeedback(feedback,
		                                         qualityOfResponse);
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

	private void callPhraseChosenEvent (Quality quality) {
		if (OnPhraseChoice != null) {
			OnPhraseChoice(quality);
		}
	}

}
