using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class PlatformController : MonoBehaviour {

	public float Speed = 0.001f;
	float initialX;
	float platformMovement = 0;
	float platformWidth;

	// Use this for initialization
	void Start () {
		setReferences();
	}
	
	// Update is called once per frame
	void Update () {
		platformMovement -= Speed;


		platformMovement %= platformWidth;

		Util.SetPosition(
			transform,
			platformMovement,
			transform.position.y,
			transform.position.z);

	}

	void setReferences () {
		// Code from: http://stackoverflow.com/questions/23535304/getting-the-width-of-a-sprite
		platformWidth = GetComponent<SpriteRenderer>().bounds.size.x;
		initialX = transform.localPosition.x;
	}

		
}
