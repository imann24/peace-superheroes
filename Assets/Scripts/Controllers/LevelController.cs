using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	public static LevelController Instance;

	public TextAsset [] LevelTemplates;

	public GameObject LevelPrefab;
	public float GroundHeight;

	private Level [] levels;
	private GameObject currentLevel;

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
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}

	private void initializeFields () {
		levels = LevelGenerator.GenerateLevels(LevelTemplates);
	}

	private void spawnLevel (int levelIndexToLoad = 0) {
		GameObject level = (GameObject) Instantiate(LevelPrefab);

		LevelSpawner spawner = level.GetComponent<LevelSpawner>();

		spawner.Initialize(levels[levelIndexToLoad]);
	}
}
