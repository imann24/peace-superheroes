using UnityEngine;
using System.Collections;

public class SceneController: MonoBehaviour {

	public void LoadPrototype () {
		Application.LoadLevel((int) Scenes.Prototype);
	}

}
