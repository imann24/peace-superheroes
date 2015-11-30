using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseCollector : MonoBehaviour {

	List<string> collectionOfPhrases = new List<string>();
	public static PhraseCollector Instance;

	void Awake () {
		Util.SingletonImplementation (
				ref Instance,
				this,
				gameObject
			);
		subscribeReferences();
	}

	void OnDestroy () {
		Util.RemoveSingleton(ref Instance);
		unsubscribeReferences();
	}

	public void CollectPhrase (string phrase) {
		collectionOfPhrases.Add(phrase);
	}

	public string UseRandomPhrase () {
		int index = Random.Range(0, collectionOfPhrases.Count);
		string phrase = collectionOfPhrases[index];
		collectionOfPhrases.RemoveAt(index);
		return phrase;
	}

	public int GetPhraseCount () {
		return collectionOfPhrases.Count;
	}

	public void UsePhrase (string phrase) {
		collectionOfPhrases.Remove(phrase);
	}

	public string [] GetAllCollectedPhrases () {
		return collectionOfPhrases.ToArray();
	}

	public void DiscardAllPhrases () {
			collectionOfPhrases.Clear();
	}

	private void subscribeReferences () {
		TrackerController.OnGameEnd += DiscardAllPhrases;
	}

	private void unsubscribeReferences () {
		TrackerController.OnGameEnd -= DiscardAllPhrases;
	}
}
