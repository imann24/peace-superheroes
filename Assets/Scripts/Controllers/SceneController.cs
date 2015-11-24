using UnityEngine;
using System.Collections;

public class SceneController: MonoBehaviour {

	public void LoadPrototype () {
		Application.LoadLevel((int) Scenes.Prototype);
	}

	public void LoadCredits () {
		Application.LoadLevel((int) Scenes.Credits);
	}

	public void LoadStartScreen () {
		Application.LoadLevel((int) Scenes.Start);
	}

	public void LoadTutorial () {
		Application.LoadLevel((int) Scenes.Tutorial);
	}

	public void LoadSite () {
		Application.OpenURL(Global.PSH_SITE_URL);
	}
}
