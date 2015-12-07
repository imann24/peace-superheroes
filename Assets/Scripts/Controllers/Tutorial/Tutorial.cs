using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {
	private const string KEY = "Tutorial";
	public static Tutorial Instance;

	public GameObject[] TutorialSlides;
	public bool TutorialPlaying {
		get; 
		private set;
	}

	private int SwipeUpIndex = 0;
	private int SwipeDownIndex = 1;
	private int ApprovePhraseIndex = 3;


	private int tutorialIndex;

	void Awake () {
		Instance = this;
		TutorialPlaying = false;
		subscribeEvents();
	}

	// Use this for initialization
	void Start () {
		if (TutorialSeen()) {
			Destroy(gameObject);
		} else {
			StartTutorial();
		}
	}

	void OnDestroy () {
		Instance = null;
		unsubscribeEvents();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartTutorial () {
		if (Application.loadedLevel == (int) Scenes.Prototype) {
			TutorialPlaying = true;
			NPCSpawnController.Instance.ToggleSpawning(false);
			TutorialSlides[0].SetActive(true);
		} else {
			Debug.LogWarning("Cannot play tutorial when not in the main game scene");
		}
	}

	public void EndTutorial () {
		TutorialPlaying = false;
		ToggleTutorialSeen(true);
		NPCSpawnController.Instance.ToggleSpawning(true);
		unitialize();
	}

	public int GetTutorialIndex () {
		return tutorialIndex;
	}

	public void StepForwardInTutorial () {
		TutorialSlides[tutorialIndex++].SetActive(false);

		if (tutorialIndex < TutorialSlides.Length) {
			if (TutorialSlides[tutorialIndex] != null) {
				TutorialSlides[tutorialIndex].SetActive(true);
			}
		} 

		if (tutorialIndex >= TutorialSlides.Length) {
			EndTutorial();
		}
	}

	public static void ToggleTutorialSeen (bool tutorialSeen) {
		PlayerPrefs.SetInt(KEY, tutorialSeen?1:0);
	}

	public static bool TutorialSeen () {
		return PlayerPrefs.GetInt(KEY, 0) == 1 ? true : false;
	}

	void subscribeEvents () {
		LaneSwitchController.OnSwitchToLane += HandleOnSwitchToLane;
		PhraseApprover.OnPhraseChoice += HandleOnPhraseChoice;
	}

	void unsubscribeEvents () {
		LaneSwitchController.OnSwitchToLane -= HandleOnSwitchToLane;
		PhraseApprover.OnPhraseChoice -= HandleOnPhraseChoice;
	}
	
	void HandleOnSwitchToLane (int lane) {
		if (TutorialPlaying) {
			if (lane == LaneSwitchController.LaneCount() - 1 && 
			    tutorialIndex == SwipeUpIndex) {

					StepForwardInTutorial();

			} else if (lane == LaneSwitchController.LaneCount() - 2 && 
				tutorialIndex == SwipeDownIndex) {

					StepForwardInTutorial();

			}
		}
	}

	
	void HandleOnPhraseChoice (Quality quality) {
		if (TutorialPlaying) {	
			StepForwardInTutorial();
		}
	}

	void unitialize () {
		Instance = null;
		Destroy(gameObject);
	}
}
