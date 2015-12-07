using UnityEngine;
using System.Collections;

public class MuteButton : MonoBehaviour {
	private bool _muted = false;

	public bool Muted { 
		set {
			_muted = value;
			AudioController.Instance.ToggleMute(_muted);
		}

		get {
			return _muted;
		}
	}
}
