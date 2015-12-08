using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlTutorial : MonoBehaviour {
	public Direction MoveDirection;
	public Text InstructionText;
	public const string TOUCH_CONTROL_VERB = "Swipe ";
	public const string KEY_CONTROL_VERB = "Press the ";
	public const string KEY_CONTROL_NOUN = " arrow ";
	public const string STOCK_INSTRUCTION = " to move ";
	// Use this for initialization

	void Start () {
		initialize();
	}

	public void SetInstructionText (string instruction) {
		InstructionText.text = instruction;
	}

	private string getText (Direction direction) {
		string verb;
		string noun = "";

#if UNITY_IOS || UNITY_ANDROID
		verb = TOUCH_CONTROL_VERB;
#else
		verb = KEY_CONTROL_VERB;
		noun = KEY_CONTROL_NOUN;
#endif

		return verb + direction + noun + STOCK_INSTRUCTION + direction;
	}

	private void initialize () {
		SetInstructionText(
			getText(MoveDirection)
			);
	}
}
