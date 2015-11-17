using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public delegate void PlayerSetAction();
	public static event PlayerSetAction OnPlayerSet;

	public delegate void PlayerOutOfBoundsAction();
	public static event PlayerOutOfBoundsAction OnPlayerOutOfBounds;

	public delegate void VictoryAction();
	public static event VictoryAction OnVictory;

	public static PlayerController Instance;
	public float Speed = 2.0f;
	public float TeleportDistance = 10f;
	
	private Rigidbody2D rigibody;
	private Animator animator;
	private Direction teleportDirection;

	private bool _readyToTeleport;
	private bool canTeleport = true;
	private bool canMove = true;
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
		callOnPlayerSet();
	}

	// Use this for initialization
	void Start () {
		setReferences();
//		LevelController.Instance.PlayerStartPosition = transform.position;
	}

	void OnDestroy () {
		Util.RemoveSingleton(ref Instance);
	}
	
	// Update is called once per frame
	void Update () {
		setMovement();
	}

	void OnCollisionEnter2D (Collision2D collision) {
		LevelPiece collideType = collisionType(collision.transform);
	
		if (collideType == LevelPiece.Ground) {
			canMove = false;
			Invoke("callOnPlayerOutOfBounds", 1.0f);
		} else if (collideType == LevelPiece.Platform) {
			canTeleport = true;
		} else if (collideType == LevelPiece.Finish) {
			callOnVictory();
		}
	}

	private void setMovement () {
//		if (canMove) {
//			transform.Translate(Vector3.right * Speed);
//		}

//		if (transform.position.y < LevelController.Instance.GroundHeight) {
//			callOnPlayerOutOfBounds();
//		}
	}

	private void setReferences () {
		animator = GetComponent<Animator>();
		rigibody = GetComponent<Rigidbody2D>();
	}

	public Transform GetTransform () {
		return transform;
	}

	public void SetTeleportDirection (Direction teleportDireciton) {
		this.teleportDirection = teleportDireciton;
		ReadyToTeleport = !(this.teleportDirection == Direction.None);

	}

	private void teleport () {
		if (canTeleport) {
			canTeleport = false;
			transform.Translate(Vector3.right * TeleportDistance/2.5f);

			switch (teleportDirection) {
				case Direction.Up:
					transform.Translate(Vector3.up * TeleportDistance);
					break;
				case Direction.Down:
					transform.Translate(Vector3.down * TeleportDistance);
					if (transform.position.y < LevelController.Instance.GroundHeight) {
						callOnPlayerOutOfBounds();
					}
					break;
				default:
					break;
			}
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
		LevelPieceController.ReactivateAllCollectibles();
		if (OnPlayerOutOfBounds != null) {
			OnPlayerOutOfBounds();
		}
		canMove = true;
	}

	private void callOnVictory () {
		if (OnVictory != null) {
			OnVictory();
		}
	}
}
