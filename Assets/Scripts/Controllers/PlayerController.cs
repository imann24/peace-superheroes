using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public delegate void PlayerSetAction();
	public static event PlayerSetAction OnPlayerSet;

	public delegate void PlayerOutOfBoundsAction();
	public static event PlayerOutOfBoundsAction OnPlayerOutOfBounds;

	public static PlayerController Instance;
	public float Speed = 2.0f;
	public float TeleportDistance = 10f;
	
	private Rigidbody2D rigibody;
	private TeleportDirection teleportDirection;

	private bool _readyToTeleport;

	public bool ReadyToTeleport {
		set {
			_readyToTeleport = value;
			if (value) {
				teleport();
			}
		}

		get {
			return _readyToTeleport;
		}
	}

	void Awake () {
		Util.SingletonImplementation(ref Instance, this, gameObject);
		OnPlayerSet();
	}

	// Use this for initialization
	void Start () {
		setReferences();
	}

	
	// Update is called once per frame
	void Update () {
		setMovement();
	}

	void OnCollisionEnter2D (Collision2D collision) {
		LevelPiece collideType = collisionType(collision.transform);

		Debug.Log(collideType);

		if (collideType == LevelPiece.Ground) {
			callOnPlayerOutOfBounds();
		}
	}

	private void setMovement () {
		rigibody.velocity = new Vector2(Speed, rigibody.velocity.y);
	}

	private void setReferences () {
		rigibody = GetComponent<Rigidbody2D>();
	}

	public Transform GetTransform () {
		return transform;
	}

	public void SetTeleportDirection (TeleportDirection teleportDireciton) {
		this.teleportDirection = teleportDireciton;
		ReadyToTeleport = !(this.teleportDirection == TeleportDirection.None);

	}

	private void teleport () {
		transform.Translate(Vector3.right * TeleportDistance);

		switch (teleportDirection) {
			case TeleportDirection.Up:
				transform.Translate(Vector3.up * TeleportDistance);
				break;
			case TeleportDirection.Down:
				transform.Translate(Vector3.down * TeleportDistance);
				if (transform.position.y < LevelController.Instance.GroundHeight) {
					callOnPlayerOutOfBounds();
				}
				break;
			default:
				break;
		}
	}

	private LevelPiece collisionType (Transform collided) {
		LevelPieceController piece = collided.parent.GetComponent<LevelPieceController>();

		if (piece == null) {
			return LevelPiece.Empty;
		} else {
			return piece.Type;
		}
	}

	private void callOnPlayerSet () {
		if (OnPlayerSet != null) {
			OnPlayerSet();
		}
	}

	private void callOnPlayerOutOfBounds () {
		if (OnPlayerOutOfBounds != null) {
			OnPlayerOutOfBounds();
		}
	}
}
