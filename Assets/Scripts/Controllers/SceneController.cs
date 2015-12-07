using UnityEngine;
using System.Collections;

public class SceneController: MonoBehaviour {
	public delegate void SceneChangeAction (Scenes scene);
	public static event SceneChangeAction OnSceneChange;

	public void LoadPrototype () {
		callSceneChangeAction(Scenes.Prototype);
		Application.LoadLevel((int) Scenes.Prototype);
	}

	public void LoadCredits () {
		callSceneChangeAction(Scenes.Credits);
		Application.LoadLevel((int) Scenes.Credits);
	}

	public void LoadStartScreen () {
		callSceneChangeAction(Scenes.Start);
		Application.LoadLevel((int) Scenes.Start);
	}

	public void LoadTutorial () {
		callSceneChangeAction(Scenes.Tutorial);
		Application.LoadLevel((int) Scenes.Tutorial);
	}

	public void LoadSite () {
		callSceneChangeAction(Scenes.Website);
		Application.OpenURL(Global.PSH_SITE_URL);
	}

	void callSceneChangeAction (Scenes scene) {
		if (OnSceneChange != null) {
			OnSceneChange(scene);
		}
	}
}
