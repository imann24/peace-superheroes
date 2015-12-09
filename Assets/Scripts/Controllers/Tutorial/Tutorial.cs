using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour {
	private const string KEY = "Tutorial";
	private const string OVERRIDE = "ForceTutorial";
	public static Tutorial Instance;
	
	public GameObject[] TutorialSlides;

	public bool TutorialPlaying {
		get; 
		private set;
	}


	public bool MovementAllowed {
		get {
			return !TutorialPlaying ||
				Util.ArrayContains(
					movementTutorialSlideIndexes,
					tutorialIndex);

		}
	}

	// For debugging purposes
	private bool testingTutorial = false;

	private int SwipeUpIndex = 7;
	private int SwipeDownIndex = 8;

	private int responsePhraseIndex = 3;
	private int conflictPhraseIndex = 4;

	private int angryNPCSpawnIndex = 3;
	private int mentorNPCSpawnIndex = 1;

	private int progressBarIndex = 5;
	private int gameEndIndex = 6;

	private int tutorialIndex;
	private int[] movementTutorialSlideIndexes = {7, 8};
	private string tutorialAdvanceMethod = "StepForwardInTutorial"; 
	private Canvas canvas;

	private List<GameObject> tutorialObjects = new List<GameObject>();

	void Awake () {
		setReferences();
		Instance = this;
		TutorialPlaying = false;
		subscribeEvents();
	}

	void setReferences () {
		canvas = GetComponent<Canvas>();
	}

	// Use this for initialization
	void Start () {
		if (TutorialSeen() &&
		    !testingTutorial && 
		    PlayerPrefs.GetInt(OVERRIDE) != 1) {
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
		resetOverride();
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
		HideCurrentSlide();

		if (tutorialIndex < TutorialSlides.Length) {
			ShowCurrentSlide();
		} 

		setUpTutorialSlide(tutorialIndex);

		if (tutorialIndex >= TutorialSlides.Length) {
			EndTutorial();
		}
	}

	public void HideCurrentSlide (bool tickToNext = true) {
		TutorialSlides[tutorialIndex].SetActive(false);
		if (tickToNext) {
			tutorialIndex++;
		}
	}

	public void ShowCurrentSlide () {
		if (TutorialSlides[tutorialIndex] != null) {
			TutorialSlides[tutorialIndex].SetActive(true);
		}
	}

	public static bool Playing () {
		return Instance != null &&
			Instance.TutorialPlaying;
	}

	public static void ToggleTutorialSeen (bool tutorialSeen) {
		PlayerPrefs.SetInt(KEY, tutorialSeen?1:0);
	}

	public static void ForceShowTutorial () {
		PlayerPrefs.SetInt(OVERRIDE, 1);
	}

	public static bool TutorialSeen () {
		return PlayerPrefs.GetInt(KEY, 0) == 1 ? true : false;
	}

	public static bool CanMove () {
		if (Instance == null) {
			return true;
		} else {
			return Instance.MovementAllowed;
		}
	}

	private void resetOverride () {
		PlayerPrefs.SetInt(OVERRIDE, 0);
	}

	private void spawnAngryNPC () {
		tutorialObjects.Add (
			NPCSpawnController.Instance.SpawnNPC(
				SpawnPoint.MidPlatform,
				Emotion.Mad,
				PhraseController.Instance.GetConflictPhrase(
					conflictPhraseIndex),
				handleNPCEncounter));
	}

	private void spawnMentorNPC () {
		tutorialObjects.Add (
			NPCSpawnController.Instance.SpawnNPC(
				SpawnPoint.MidPlatform,
				Emotion.None,
				PhraseController.Instance.GetPhrase(
					responsePhraseIndex),
				handleNPCEncounter));
	}

	private void setUpTutorialSlide (int index) {
		if (index == mentorNPCSpawnIndex) {
			spawnMentorNPC();
		} else if (index == gameEndIndex) {
			timedTickTutorial();
		} else if (index >= SwipeUpIndex) {
			CancelInvoke();
		}

	}

	void subscribeEvents () {
		LaneSwitchController.OnSwitchToLane += HandleOnSwitchToLane;
		PhraseApprover.OnPhraseChoice += HandleOnPhraseChoice;
		PhraseSelector.OnPhraseChoice += HandlePhraseSelected;
		PhraseFeedback.OnFeedbackClosed += HandleOnFeedbackClosed;
	}

	void unsubscribeEvents () {
		LaneSwitchController.OnSwitchToLane -= HandleOnSwitchToLane;
		PhraseApprover.OnPhraseChoice -= HandleOnPhraseChoice;
		PhraseSelector.OnPhraseChoice -= HandlePhraseSelected;
		PhraseFeedback.OnFeedbackClosed -= HandleOnFeedbackClosed;
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


	void HandleOnFeedbackClosed (Quality quality) {
		ShowCurrentSlide();
		timedTickTutorial();
	}

	void HandleOnPhraseChoice (Quality quality) {
		if (TutorialPlaying) {
			spawnAngryNPC();
			HideCurrentSlide();
		}
	}

	void HandlePhraseSelected (Quality quality) {
		HideCurrentSlide();
		tutorialIndex = progressBarIndex;
		CancelInvoke();
	}

	void handleNPCEncounter (Emotion emotion, string phrase, NPCController npc) {
		canvas.sortingOrder++;
	
		if (emotion == Emotion.None) {
			PhraseApprover.Instance.ToggleRejectButton(false);
			StepForwardInTutorial();
		} else if (emotion == Emotion.Mad) {
			ShowCurrentSlide();
			timedTickTutorial(5f);
		}
	}

	void unitialize () {
		Instance = null;
		foreach (GameObject tutorialObject in tutorialObjects) {
			Destroy(tutorialObject);
		}
		Destroy(gameObject);
	}

	void timedTickTutorial (float time = 8.0f) {
		Invoke(tutorialAdvanceMethod, time);
	}
}
