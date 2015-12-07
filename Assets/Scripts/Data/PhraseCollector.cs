using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhraseCollector : MonoBehaviour, System.IComparable<PhraseCollector> {

	List<string> collectionOfPhrases = new List<string>();
	public static PhraseCollector Instance;

	public static int Count = 0;
	private int ID = Count++;

	void Awake () {
		Util.SingletonImplementation (
				ref Instance,
				this,
				gameObject
			);
		subscribeReferences();
	}

	void OnDestroy () {
		Util.RemoveSingleton(ref Instance, this);
		unsubscribeReferences();
	}

	public void CollectPhrase (string phrase) {
		if (string.IsNullOrEmpty(phrase)) {
			collectionOfPhrases.Add(
				PhraseController.Instance.GetRandomPhrase());
		} else {
			collectionOfPhrases.Add(phrase);
		}
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

	public int CompareTo (PhraseCollector other) {
		return other.ID == this.ID ? 0 : -1;
	}

	private void subscribeReferences () {
		TrackerController.OnGameEnd += DiscardAllPhrases;
	}

	private void unsubscribeReferences () {
		TrackerController.OnGameEnd -= DiscardAllPhrases;
	}
}
