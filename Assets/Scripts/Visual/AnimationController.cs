using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class AnimationController : MonoBehaviour {
	public string CelebrationAnimationTrigger = "Celebrate";
	public string SadnessAnimationTrigger = "Sad";

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
		if (Input.GetKeyDown(KeyCode.Space)) {
			playSadAnimation();
		}
	}

	void subscribeEvents () {
		MovementController.OnGameStateChanged += toggleAnimation;
		PhraseFeedback.OnFeedbackClosed += handlePhraseSelection;
		TrackerController.OnCharacterReaction += handlePhraseSelection;
	}

	void unsubscribeEvents () {
		MovementController.OnGameStateChanged -= toggleAnimation;
		PhraseFeedback.OnFeedbackClosed -= handlePhraseSelection;
		TrackerController.OnCharacterReaction -= handlePhraseSelection;
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
		} else if (quality == Quality.Bad) {
			playSadAnimation();
		}
	}

	void playCelebrationAnimation () {
		animator.SetTrigger(CelebrationAnimationTrigger);
	}

	void playSadAnimation () {
		animator.SetTrigger(SadnessAnimationTrigger);
	}
}
