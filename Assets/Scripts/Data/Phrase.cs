using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phrase : MonoBehaviour {

	public delegate void PhraseCollectedAction (string phrase);
	public static event PhraseCollectedAction OnPhraseCollected;

	public static Dictionary<int, Phrase> All = new Dictionary<int, Phrase>();
	public static int Count = 0;

	public string phrase;

	public TextMesh visualPhrase;

	private int index;

	// Use this for initialization
	void Start () {
	}
	
	void OnDestroy () {
		All.Remove(index);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == Tags.PLAYER) {
			callPhraseCollectedEvent();
			Destroy(gameObject);
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
		} catch {
			All.Remove(Count);
			All.Add(Count, this);
		} finally {
			index = Count++;
		}
	}

	private void callPhraseCollectedEvent () {
		if (OnPhraseCollected != null) {
			OnPhraseCollected(this.phrase);
		}
	}

}
