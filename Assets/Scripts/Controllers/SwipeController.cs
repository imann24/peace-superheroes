﻿// Script from http://pfonseca.com/swipe-detection-on-unity/
using UnityEngine;
using System.Collections;

public class SwipeController : MonoBehaviour {
	
	public delegate void SwipeAction (Direction direction);
	public static event SwipeAction OnSwipe;

	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
	
	// calls swipe event
	void callSwipeEvent (Direction direction) {
		if (OnSwipe != null) {
			OnSwipe(direction);
		}
	}
	// Update is called once per frame
	void Update () {
		if (MovementController.Instance.Paused) {
			return;
		}

		if (Input.touchCount > 0){
			
			foreach (Touch touch in Input.touches)
			{
				switch (touch.phase)
				{
				case TouchPhase.Began :
					/* this is a new touch */
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;
					
				case TouchPhase.Canceled :
					/* The touch is being canceled */
					isSwipe = false;
					break;
					
				case TouchPhase.Ended :
					
					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;
					
					if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
						Vector2 direction = touch.position - fingerStartPos;
						Vector2 swipeType = Vector2.zero;
						
						if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
							// the swipe is horizontal:
							swipeType = Vector2.right * Mathf.Sign(direction.x);
						}else{
							// the swipe is vertical:
							swipeType = Vector2.up * Mathf.Sign(direction.y);
						}
						
						if(swipeType.x != 0.0f){
							if(swipeType.x > 0.0f){
								// MOVE RIGHT
							}else{
								// MOVE LEFT
							}
						}
						
						if(swipeType.y != 0.0f ){
							if(swipeType.y > 0.0f){
								// MOVE UP
								callSwipeEvent(Direction.Up);
							}else{
								callSwipeEvent(Direction.Down);
								// MOVE DOWN
							}
						}
						
					}
					
					break;
				}
			}
		}
		
	}
}