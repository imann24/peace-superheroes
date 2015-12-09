using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(GridLayoutGroup))]

public class GridLayoutScaler : MonoBehaviour {
	GridLayoutGroup gridLayout;
	Canvas canvas;
	PhraseSelector selector;

	void Awake () {
		setReferences();
	}

	void OnEnable () {
		setReferences();
		scaleGridLayoutElements();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void setReferences () {
		gridLayout = GetComponent<GridLayoutGroup>();
		canvas = GetComponentInParent<Canvas>();
		selector = GetComponentInParent<PhraseSelector>();
	}

	void scaleGridLayoutElements () {
		gridLayout.cellSize = new Vector2(
			Screen.width/2.5f,
			Screen.height/8.0f);

		gridLayout.padding = new RectOffset(
			0, 0, Screen.width/32, 0);

		gridLayout.spacing = new Vector2(
			0,
			Screen.height/128.0f);

		selector.ScaleScrollRect();

	}
	
}
