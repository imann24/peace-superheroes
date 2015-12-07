using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class AnimationController : MonoBehaviour {
	public string CelebrationAnimationTrigger = "Celebrate";
	Animator animator;
	// Use this for initialization
	void Start () {
		setReferences();
		subscribeEvents();
	}


	void OnDestroy () {
		unsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
	}

	void subscribeEvents () {
		MovementController.OnGameStateChanged += toggleAnimation;
		PhraseApprover.OnPhraseChoice += handlePhraseSelection;
		PhraseSelector.OnPhraseChoice += handlePhraseSelection;
	}

	void unsubscribeEvents () {
		MovementController.OnGameStateChanged -= toggleAnimation;
		PhraseApprover.OnPhraseChoice -= handlePhraseSelection;
		PhraseSelector.OnPhraseChoice -= handlePhraseSelection;
	}

	void toggleAnimation (GameState newState) {
		if (newState == GameState.Game) {
			animator.StopPlayback();
		} else {
			animator.StartPlayback();
		}
	}

	void setReferences () {
		animator = GetComponent<Animator>();
	}

	void handlePhraseSelection (Quality quality) {
		if (quality == Quality.Great ||
		    quality == Quality.Good) {
			playCelebrationAnimation();
		}
	}

	void playCelebrationAnimation () {
		animator.SetTrigger(CelebrationAnimationTrigger);
	}
}
