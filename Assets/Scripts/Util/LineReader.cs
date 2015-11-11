using UnityEngine;
using System.Collections;

public class LineReader {
	public static string [] ReadByLine (TextAsset document) {
		return document.text.Split('\n');
	}
}
