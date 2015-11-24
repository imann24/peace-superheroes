using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	//public static event 
	public static MovementController Instance;
	private bool _paused;
	public bool Paused {
		get {
			return _paused;
		}

		set {
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
}
