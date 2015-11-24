// Has a UI match the position of a gameobject with a given offset

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RectTransform))]

public class TrackUIWithGameObject : MonoBehaviour {
	public delegate void FunctionToSubscribe();
	public delegate void FinishedTrackingAction(GameObject uiObject);
	public event FinishedTrackingAction OnTrackingFinished;

	public float OffsetFraciton = 0.5f;
	public Vector2 Offset;
	public GameObject ObjecToTrack;
	public GameObject ParentCanvas;

	private RectTransform rectTransform;
	private RectTransform canvasRect;
	// Use this for initialization
	void Start () {
		setReferences();
	}
	
	// Update is called once per frame
	void Update () {
		trackGameObject();
	}


	// Code from: http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
	void trackGameObject () {
		if (ObjecToTrack == null ||
		    ParentCanvas == null) {
			return;
		}

		//then you calculate the position of the UI element
		//0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
		Vector2 ViewportPosition=Camera.main.WorldToViewportPoint(ObjecToTrack.transform.position);
		Vector2 WorldObject_ScreenPosition=new Vector2(
			((ViewportPosition.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
			((ViewportPosition.y*canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)))
			+ Offset;
		
		//now you can set the position of the ui element
		rectTransform.anchoredPosition=WorldObject_ScreenPosition;
	}

	void setReferences () {
		rectTransform = GetComponent<RectTransform>();
		Offset.y = (float)Screen.height/(float)Screen.width * OffsetFraciton;
		SetParentRect();
	}

	public void SetParentRect (GameObject parentCanvas = null) {
		if (parentCanvas != null) {
			this.ParentCanvas = parentCanvas;
		}

		if (this.ParentCanvas != null) {
			this.canvasRect = this.ParentCanvas.GetComponent<RectTransform>();
		}
	}

	public void SetObjecToTrack (GameObject objectToTrack) {
		this.ObjecToTrack = objectToTrack;

		NPCController npc = GetComponent<NPCController>();

		if (npc != null) {
			npc.OnOffscreen += (GameObject g, SpawnPoint spawnPoint) => callFinishedTrackingEvent();
		}
	}

	public void SetOffset (Vector2 offset) {
		this.Offset = offset;
	}

	public void SubscribeEvent (FunctionToSubscribe function) {
		function += callFinishedTrackingEvent;
	}

	private void callFinishedTrackingEvent () {
		if (OnTrackingFinished != null) {
			OnTrackingFinished(gameObject);
		}
	}
}
