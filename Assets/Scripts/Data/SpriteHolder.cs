using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteHolder : MonoBehaviour {
	public Sprite AngryNPC;
	public Sprite CalmNPC;

	private static Dictionary <Sprites, Sprite> All = new Dictionary<Sprites, Sprite>();

	void Awake () {
		initialize();
	}

	// Use this for initialization
	void Start () {
	
	}

	void OnDestroy () {
		All.Clear();
	}

	void initialize () {
		All.Clear();
		All.Add(Sprites.AngryNPC, AngryNPC);
		All.Add(Sprites.CalmNPC, CalmNPC);
	}

	public static Sprite GetSprite (Sprites sprite) {
		if (All.ContainsKey(sprite)) {
			return All[sprite];
		} else {
			return null;
		}
	}
}
