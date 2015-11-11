using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {
	private const string UpKey = "Up";
	private const string DownKey = "Down";


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
