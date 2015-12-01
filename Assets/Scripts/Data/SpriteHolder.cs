using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteHolder : MonoBehaviour {
	public Sprite AngryNPC;
	public Sprite AngrierNPC;
	public Sprite CalmNPC;
	public Sprite MentorNPC;

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
		All.Add(Sprites.AngrierNPC, AngrierNPC);
		All.Add(Sprites.CalmNPC, CalmNPC);
		All.Add(Sprites.MentorNPC, MentorNPC);
	}

	public static Sprite GetSprite (Sprites sprite) {
		if (All.ContainsKey(sprite)) {
			return All[sprite];
		} else {
			return null;
		}
	}
}
