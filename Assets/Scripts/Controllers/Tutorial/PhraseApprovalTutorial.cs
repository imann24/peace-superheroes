using UnityEngine;
using System.Collections;

public class PhraseApprovalTutorial : MonoBehaviour {
	public int TutorialPhraseIndex = 5;
	public GameObject PhraseApproverCanvas;

	// Use this for initialization
	void Start () {
		ActivateTutorialPhraseApproval();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ActivateTutorialPhraseApproval () {
		PhraseApproverCanvas.SetActive(true);
		PhraseApprover.Instance.SetPhrase(
			PhraseController.Instance.GetResponsePhrase(
				TutorialPhraseIndex));
		PhraseApprover.Instance.ToggleRejectButton(false);
	}
}
