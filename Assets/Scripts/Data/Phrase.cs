using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phrase : MonoBehaviour {
	public static Dictionary<int, Phrase> All = new Dictionary<int, Phrase>();
	public static int Count = 0;

	public string phrase;

	public TextMesh visualPhrase;

	private int index;

	// Use this for initialization
	void Start () {
	}
	
	void OnDestroy () {
		if (All.Count > 0) {
			All.Clear();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize () {
		addToDictionary();
	}

	public void SetPhrase (string phrase) {
		this.phrase = phrase;
		SetVisualPhrase();
	}

	public void SetVisualPhrase () {
		visualPhrase.text = this.phrase;
	}

	public int GetIndex () {
		return index;
	}

	private void addToDictionary () {
		try {
			All.Add(Count, this);
		} finally {
			index = Count++;
		}
	}

}
