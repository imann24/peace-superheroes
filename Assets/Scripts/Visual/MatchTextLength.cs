using UnityEngine;
using System.Collections;

public class MatchTextLength : MonoBehaviour {

	public TextMesh Text;

	private SpriteRenderer sprite;


	void Awake () {
		setReferences();
	}

	// Use this for initialization
	void Start () {
		setWidth();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setWidth (float xOffsetScale = 0.0f) {
		try {
			Vector2 currentScale = sprite.transform.localScale;
			sprite.transform.localScale = new Vector2(getSpriteWidthFromTextMeshWidth(Text),
			                                          currentScale.y);
			sprite.transform.position += Vector3.right * getSpriteWidthFromTextMeshWidth(Text) * xOffsetScale;
		} catch {
			Debug.LogError("SpriteRenderer reference not set");
		}
	}

	void setReferences () {
		sprite = GetComponent<SpriteRenderer>();
	}

	float getSpriteWidthFromTextMeshWidth (TextMesh mesh, float scalingFactor = 3.0f) {
		Vector2 currentScale = sprite.transform.localScale;
		return mesh.characterSize * mesh.text.Length * currentScale.x * scalingFactor;
	}
}
