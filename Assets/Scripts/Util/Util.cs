﻿using System;
using UnityEngine;
using System.Collections;

public class Util {

	public static bool SingletonImplementation<T> (ref T staticInstance, T instance, GameObject associatedObject)
	where T : System.IComparable<T> {
		if (staticInstance == null) {
			UnityEngine.Object.DontDestroyOnLoad(associatedObject);
			staticInstance = instance;
			return true;
		} else if (staticInstance.CompareTo(instance) != 0) {
			UnityEngine.Object.Destroy(associatedObject);
			return false;
		} else {
			return false;
		}
	}

	public static void RemoveSingleton<T> (ref T staticInstance, T instance)
	where T :System.IComparable<T> {
		if (staticInstance != null &&
		    instance != null &&
			staticInstance.CompareTo(instance) == 0) {
				staticInstance = default(T);
		}
	}

	public static string RemoveSpaces (string targetString) {
		if (string.IsNullOrEmpty(targetString)) {
			return targetString;
		}

		targetString =  targetString.Replace('\r', '\0');
		return targetString[targetString.Length-1] == '\0'?targetString.Substring(0, targetString.Length-1):targetString;
	}

	public static void ToggleHalo (GameObject targetObject, bool active) {
		(targetObject.GetComponent("Halo") as Behaviour).enabled = active;
	}

	public static Vector3 WorldPositionFromMouse () {
		Vector3 currentMousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
		return Camera.main.ScreenToWorldPoint (currentMousePosition);
	}

	public static Vector3 MatchPosition (Transform leader, Transform follower, bool x, bool y, bool z) {
		return new Vector3 (
			x ? leader.position.x : follower.position.x,
			y ? leader.position.y : follower.position.y,
			z ? leader.position.z : follower.position.z);
	}


	public static Vector3 RoundPositionToNearestHalves (Vector3 position) {
		position.x = RoundToNearestFifth(position.x);
		position.y = RoundToNearestFifth(position.y);
		position.z = RoundToNearestFifth(position.z);

		return position;

	}

	public static void SetPosition (Transform transform, float x, float y, float z) {
		transform.position = new Vector3(x, y, z);
	}
	

	//Code from http://stackoverflow.com/questions/1329426/how-do-i-round-to-the-nearest-0-5
	public static float RoundToNearestFifth (float value) {
		return (float) Math.Round(value * 2f, MidpointRounding.AwayFromZero)/2f;
	}

	// Generic method returns true or false if the array contains the object
	public static bool ArrayContains<T> (T [] arrayToSearch, T objectToFind) where T : System.IComparable<T> {
		return System.Array.Exists(arrayToSearch, s => {if( s.CompareTo(objectToFind) == 0) 
			return true;
			else 
				return false;
		});
	}

	public static T [][] InitializeMatrixAsJaggedArray<T> (T[][] matrix, int w, int h) {
		matrix = new T[w][];

		for (int x = 0; x < w; x++) { 
			matrix[x] = new T[h];
		}

		return matrix;
	}

	public static int MatrixHeight<T> (T[][] matrix) {
		int maxHeight = 0;
		for (int i = 0; i < matrix.Length; i++) {
			if (matrix[i].Length > maxHeight) {
				maxHeight = matrix[i].Length;
			}
		}
		return maxHeight;
	}

	public static bool WithinRange (int value, int upperBound, int lowerBound =0, bool includeUpperBound = false) {
		bool withinUpperBound = includeUpperBound?value<=upperBound:value<upperBound;
		return withinUpperBound && value >= lowerBound;
	}

	public static T [] RemoveNullElements <T> (T[] array) {
		int newLength = array.Length;
		int index = 0;
		T[] newArray;
		bool [] nullElements = new bool[array.Length];

		for (int i = 0; i < array.Length; i++) {
			nullElements[i] = array[i] == null;
			newLength = nullElements[i]?newLength-1:newLength;
		}

		newArray = new T[newLength];

		for (int i = 0; i < array.Length; i++) {
			if (nullElements[i]) {
				continue;
			} else {
				newArray[index++] = array[i];
			}
		}

		return newArray;
	}

	public static Color ChangeColorOpacty (Color color, float opacity) {
		return new Color(color.r, color.g, color.b, opacity);
	}

	public static void DeleteAllChildren (Transform transform) {
		for (int i = 0; i < transform.childCount; i++) {
			UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
		}
	}

	public static string TwoDimensionArrayToString<T> (T[,] array) {
		string arrayAsString = "";


		for (int y = 0; y < array.GetLength(1); y++){
			for (int x = 0; x < array.GetLength(0); x++) {
				arrayAsString += array[x, y];
			}

			arrayAsString += "\n";
		}

		return arrayAsString;
	}

	public static bool IsSquareJaggedArray<T> (T[][] source) {
		int same = source[0].Length;

		for (int i = 1; i < source.Length; i++) {
			if (same != source[i].Length) {
				return false;
			}
		}

		return true;
	}

	// Jagged Array must be square
	public static T[,] To2DArray <T>(T[][] source) {
		if (!IsSquareJaggedArray(source)) {
			Debug.LogError("Jagged array is not square");
			return null;
		}

		T[,] output = new T[source.Length, source[0].Length];

		for (int x = 0; x < source.Length; x++) {
			for (int y = 0; y < source[0].Length; y++) {
				output[x,y] = source[x][y];
			}
		}

		return output;
	
	}

	public static T RandomElement<T> (T[] source) {
		return source[UnityEngine.Random.Range(0, source.Length)];
	}
}
