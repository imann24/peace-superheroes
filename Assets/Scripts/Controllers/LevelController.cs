﻿using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour, System.IComparable<LevelController> {
	public static LevelController Instance;

	public TextAsset [] LevelTemplates;

	public GameObject LevelPrefab;
	public float GroundHeight;
	public Vector2 PlayerStartPosition;

	private Level [] levels;
	private GameObject currentLevel;

	public static int Count = 0;
	private int ID = Count++;

	void Awake () {
		Util.SingletonImplementation(
			ref Instance,
			this,
			gameObject);
	}

	// Use this for initialization
	void Start () {
		initializeFields();
		spawnLevel();
		subscribeEvents();
	}

	void OnDestroy () {
		unsubscribeEvents();
	}

	// Update is called once per frame
	void Update () {
	
	
	}

	public int CompareTo (LevelController other) {
		return other.ID == this.ID ? 0 : -1;
	}

	private void initializeFields () {
		levels = LevelGenerator.GenerateLevels(LevelTemplates);
	}

	private void spawnLevel (int levelIndexToLoad = 0) {
		GameObject level = (GameObject) Instantiate(LevelPrefab);

		LevelSpawner spawner = level.GetComponent<LevelSpawner>();

		spawner.Initialize(levels[levelIndexToLoad]);
	}

	private void resetPlayerPosition () {
		PlayerController.Instance.GetTransform().position = PlayerStartPosition;
	}

	void subscribeEvents () {
		PlayerController.OnPlayerOutOfBounds += resetPlayerPosition;
	}

	void unsubscribeEvents () {
		PlayerController.OnPlayerOutOfBounds -= resetPlayerPosition;
	}
}
