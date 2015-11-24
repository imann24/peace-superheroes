using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Animator))]

public class AnimationController : MonoBehaviour {
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
	}

	void unsubscribeEvents () {
		MovementController.OnGameStateChanged -= toggleAnimation;
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
}
