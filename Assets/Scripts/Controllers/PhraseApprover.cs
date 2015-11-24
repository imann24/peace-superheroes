using UnityEngine;
using System.Collections;

public class PhraseApprover : MonoBehaviour {
	public static PhraseApprover Instance;

	public PhraseAnimation phraseAnimator;

	private string currentPhrase;


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
	}

	public void GatherPhrase () {
		TrackerController.Instance.collectPhrase(currentPhrase);
		Debug.Log("Phrase gathered");
		currentPhrase = null;
	}

	public bool HasPhrase () {
		return currentPhrase == null;
	}

	public void ApprovePhrase () {
		phraseAnimator.StartAnimation(Direction.Up);
	}

	public void RejectPhrase () {
		phraseAnimator.StartAnimation(Direction.Down);
		currentPhrase = null;
	}

	void subscribeEvents () {
		PhraseAnimation.OnPhraseCollected += GatherPhrase;
	}

	void unsubscribeEvents () {
		PhraseAnimation.OnPhraseCollected -= GatherPhrase;
	}

}
