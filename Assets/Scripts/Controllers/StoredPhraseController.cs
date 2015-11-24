using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Button))]

public class StoredPhraseController : MonoBehaviour {
	public delegate void PhraseSelectedAction (string phrase);
	public static event PhraseSelectedAction OnPhraseSelected;

	public Text text;
	string phrase;
	Button button;

	void Awake () {
		setReferences();
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText (string phrase) {
		this.phrase = phrase;
		setButtonAction(phrase);
		setVisualText();
	}
	
	public string GetText () {
		return this.phrase;
	}

	private void setVisualText () {
		text.text = phrase;
	}

	private void setButtonAction (string phrase) {
		button.onClick.AddListener(() => {
			callPhraseSelectedEvent();
			PhraseSelector.Instance.CloseSelector();
			PhraseCollector.Instance.UsePhrase(phrase);
			TrackerController.Instance.UsePhraseCorrectly();
		});
	}

	private void setReferences () {
		button = GetComponent<Button>();
	}

	private void callPhraseSelectedEvent () {
		if (OnPhraseSelected != null) {
			OnPhraseSelected(phrase);

		}
	}

}
