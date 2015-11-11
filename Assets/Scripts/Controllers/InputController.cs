using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private const string UpKey = "up";
	private const string DownKey = "down";


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(UpKey)) {
			SetTeleportDirection(UpKey);
		}

		if (Input.GetKeyDown(DownKey)) {
			SetTeleportDirection(DownKey);
		}
	}

	public void SetTeleportDirection (string direction) {
		switch(direction) {
			case UpKey:
				PlayerController.Instance.SetTeleportDirection(TeleportDirection.Up);
				break;
			case DownKey:
				PlayerController.Instance.SetTeleportDirection(TeleportDirection.Down);
				break;
			default:
				PlayerController.Instance.SetTeleportDirection(TeleportDirection.None);
				break;
		}

	}

	void subscribeReferences () {

	}

	void unsubscribeReferences () {

	}
}
