using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhraseApprover : MonoBehaviour {
	public delegate void PhraseChoiceAction (Quality quality = Quality.Good);
	public static event PhraseChoiceAction OnPhraseChoice;

	public static PhraseApprover Instance;

	public PhraseAnimation phraseAnimator;

	private string currentPhrase;

	public Text Phrase;

	void Awake () {
		Instance = this;
		subscribeEvents();
	}

	// Use this for initialization
	void Start () {
	
	}

	void OnDestroy () {
		unsubscribeEvents();
		Instance = null;
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetPhrase (string phrase) {
		currentPhrase = phrase;
		Phrase.text = phrase;

	}

	public void GatherPhrase () {
		TrackerController.Instance.collectPhrase(currentPhrase);
		currentPhrase = null;
	}

	public bool HasPhrase () {
		return currentPhrase == null;
	}

	public void ApprovePhrase () {
		phraseAnimator.StartAnimation(Direction.Up);
		MovementController.Instance.Paused = false;
		PhraseCollector.Instance.CollectPhrase(currentPhrase);
		callPhraseChoiceEvent(Quality.Good);
	}

	public void RejectPhrase () {
		phraseAnimator.StartAnimation(Direction.Down);
		currentPhrase = null;
		MovementController.Instance.Paused = false;
		callPhraseChoiceEvent(Quality.Bad);
	}

	void subscribeEvents () {
		PhraseAnimation.OnPhraseCollected += GatherPhrase;
	}

	void unsubscribeEvents () {
		PhraseAnimation.OnPhraseCollected -= GatherPhrase;
	}

	void callPhraseChoiceEvent (Quality quality) {
		if (OnPhraseChoice != null) {
			OnPhraseChoice(quality);
		}
	}

}
