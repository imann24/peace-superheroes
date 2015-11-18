// Used to detect plusses for the game color plus
// Isaiah Mann 2015 ibm13@hampshire.edu

using UnityEngine;
using System.Collections;

public class DetectPlus : MonoBehaviour {
	// Dimensions of the grid
	public int width = 8;
	public int height = 5;

	// Array of all the cubes
	public GameObject [,] AllCubes;

	// Array of all the valid colors
	public Color [] allColors = {
		Color.blue,
		Color.green,
		Color.red,
		Color.yellow,
		Color.magenta
	};

	// Use this for initialization
	void Start () {
		AllCubes = new GameObject[width, height];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Returns an enum type based on whether a plus is detected
	public PlusType ValidPlus (int centerX, int centerY) {

		// Checking that the center cube is within bounds
		// Excludes cubes on the sides
		if (inBounds(centerX, centerY)) {
			Color center = AllCubes[centerX,centerY].GetComponent<Material>().color;
			Color left = AllCubes[centerX-1,centerY].GetComponent<Material>().color;
			Color right = AllCubes[centerX+1,centerY].GetComponent<Material>().color;
			Color above = AllCubes[centerX,centerY+1].GetComponent<Material>().color;
			Color below = AllCubes[centerX,centerY-1].GetComponent<Material>().color;
		
			// Returns the plustype that is detected
			return (checkForPlusType(
				center,
				left,
				right,
				above,
				below));
		} else {

			// Returns none if out of bounds
			return PlusType.None;
		}


	}

	// Function to check whether the coordinates are a valid center point for a plus
	private bool inBounds (int centerX, int centerY) {
		if (centerX > 0 && centerX < width - 1 &&
		    centerY > 0 && centerY < height - 1) {
			return true;
		} else {
			return false;
		}
	}

	// To check for the plus types based on the colors 
	private PlusType checkForPlusType (
		Color center,
		Color left,
		Color right,
		Color above,
		Color below
		) {

		// Checks for plus types of the same color
		if (center == left &&
		    center == right &&
		    center == above &&
		    center == below) {
			return PlusType.OneColor;
		} 


		// Checks for plus types of all the colors
		else if (
			AllColors(
				center,
				left,
				right,
				above,
				below
			)) {

			return PlusType.AllColors;
		}

		// Returns none if neither is found
		else {
			return PlusType.None;
		}
	
	}

	// Used to detect whether every color is present in the plus
	private bool AllColors (Color center,
	                        Color left,
	                        Color right,
	                        Color above,
	                        Color below) {

		// booleans to correspond to each color that is used in the game
		bool[]colorsSeen = new bool[allColors.Length];

		// Putting the colors in the plus into an array for easier iteration
		Color[]colorsToCheck = {center, left, right, above, below};


		// Loops through each color that exists in the game
		for (int i = 0; i < allColors.Length; i++) {

			// Checks each color in the plus against that color 
			for (int j = 0; j < colorsToCheck.Length; j++) {


				// Sets the bool corresponding to the in game color to true
				// This correspondence is based on their index in the array
				if (colorsToCheck[j] == allColors[i]) {
					colorsSeen[i] = true;
				}
			
			}
		}

		// A count of which colors were seen
		int countColorsSeen = 0;

		// Loops throw the bool array and counts how many colors were seen
		for (int i = 0; i < colorsSeen.Length; i++) {
			if (colorsSeen[i]) {
				countColorsSeen++;
			}
		}


		// Returns true if we saw as many colors as exist in the game
		if (countColorsSeen == allColors.Length) {
			return true;
		} 

		// Otherwise returns false
		else {
			return false;
		}


	}

}
