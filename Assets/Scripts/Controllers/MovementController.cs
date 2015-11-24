using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {
	public delegate void GameStateChangeAction (GameState newState);
	public static event GameStateChangeAction OnGameStateChanged;

	public static MovementController Instance;
	private bool _paused = false;
	public bool Paused {
		get {
			return _paused;
		}

		set {
			if (_paused != value) {
				callGameStateChangeEvent (value ?
				                          GameState.GamePaused:
				                          GameState.Game);
			}
			_paused = value;

		}

	}

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	}

	void OnDestroy () {
		Instance = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void callGameStateChangeEvent (GameState newState) {
		if (OnGameStateChanged != null) {
			OnGameStateChanged(newState);
		}
	}

	public void TogglePause (bool paused) {
		Paused = paused;
	}
}
