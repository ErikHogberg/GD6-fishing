using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerDog2Script : MonoBehaviour {

	public DogPlatformerHandScript Player;
	public SpriteRenderer Leash;
	private float leashInitWidth;

	public enum DogState {
		Normal,
		Distracted,
		Exhausted,
		Jumping
	}

	private DogState State = DogState.Normal;
	float timer = -1;

	[HideInInspector]
	public Rigidbody2D DogRB;
	private SpriteRenderer dogSprite;
	private Animator dogAnimator;
	private Color initColor;

	public bool Follow = true;
	public float FollowDistance;
	public float FollowSpeed;

	// public float Trust = .5f;
	// private bool distracted = false;
	private GameObject squirrel = null;

	public float SpriteFlipThreshold = 0.01f;

	private float trust = .5f;

	public float LeashLength = 1f;

	[Header("Trust")]
	public float GrumpyThreshold = .1f;
	public float DistractedThreshold = .4f;
	public float FearThreshold = .6f;
	public float PanicThreshold = .6f;
	// public float ObedientThreshold = 1f;

	[Space]
	public float PanicTime = 1;
	public float PanicSpeed = 1;
	public Vector2 PanicTurnTimeRange = Vector2.one;
	private float fatigue = -1;
	private float panicTimer = -1;
	private float panicTurnTimer = -1;
	private float panicDir = 1;

	public bool IsPanicking => panicTimer > 0;

	public float ExhaustRate = 1;

	[Space]
	public float JumpForce = 10;
	public float JumpDelay = .1f;
	public float JumpDelayIfStanding = .1f;
	private float queuedJumpAngle;
	public AnimationCurve JumpOffsetCurve;
	public float JumpOffsetMul = 1;
	public float JumpMaxHandDistance;

	[Space]
	public Vector3 RaycastPos;
	public float RaycastDistance;
	public LayerMask RaycastMask;
	public Vector3 TeleportOffset;

	void Start() {
		DogRB = GetComponent<Rigidbody2D>();
		dogSprite = GetComponent<SpriteRenderer>();
		dogAnimator = GetComponent<Animator>();
		initColor = dogSprite.color;

		if (Leash)
			leashInitWidth = Leash.size.x;
	}

	private void Update() {

		UpdateLeash();

		if (timer > 0) {
			timer -= Time.deltaTime;
			if (timer <= 0) {
				switch (State) {
					case DogState.Normal:
					case DogState.Distracted:
					case DogState.Exhausted:
						break;
					case DogState.Jumping:
						State = DogState.Normal;
						DogRB.AddForce(
							new Vector2(
								Mathf.Cos(Mathf.Deg2Rad * queuedJumpAngle),
								Mathf.Sin(Mathf.Deg2Rad * queuedJumpAngle)
							) * JumpForce,
							ForceMode2D.Impulse
						);
						// Debug.Log(
						// 	"Dog jump. angle: " + queuedJumpAngle
						// 	+ ", cos: " + Mathf.Cos(Mathf.Deg2Rad * queuedJumpAngle)
						// 	+ ", sin: " + Mathf.Sin(Mathf.Deg2Rad * queuedJumpAngle)
						// );
						break;
						// default:
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			AddTrust(-.05f);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			AddTrust(.05f);
		}
	}

	private void FixedUpdate() {
		if (DogRB.velocity.x > SpriteFlipThreshold) {
			dogSprite.flipX = true;
		} else
		if (DogRB.velocity.x < -SpriteFlipThreshold) {
			dogSprite.flipX = false;
		}

		dogAnimator.SetFloat("MoveSpeed", Mathf.Abs(DogRB.velocity.x));

		if (panicTimer > 0) {
			panicTimer -= Time.deltaTime;
			if (panicTurnTimer < 0) {
				panicDir *= -1f;
				panicTurnTimer = Random.Range(PanicTurnTimeRange.x, PanicTurnTimeRange.y);
			}

			Vector2 velocity = DogRB.velocity;
			velocity.x = PanicSpeed * panicDir;
			DogRB.velocity = velocity;
			// DogRB.AddForce(Vector2.right * PanicSpeed * panicDir, ForceMode2D.Force);
		}

		Vector2 delta = Player.transform.position - transform.position;
		// float sqrDistance = delta.sqrMagnitude;
		if (delta.y > LeashLength) {
			// TODO: lose trust when lifting dog off ground with leash
			// IDEA: make dog enter exhausted state
			AddTrust(-trust);
			// Debug.Log("Dog is hanging off leash");
			dogSprite.color = Color.red;
		} else if (Mathf.Abs(delta.x) > LeashLength) {
			// Debug.Log("Dog is pulled by leash");
			dogSprite.color = Color.yellow;
			AddTrust(-.01f * Time.deltaTime);
			if (fatigue >= 0) {
				fatigue -= ExhaustRate * Time.deltaTime;
				if (fatigue < 0) {
					squirrel = null;
					State = DogState.Exhausted;
					dogAnimator.SetBool("Sitting", true);
				}
			}
		} else {
			dogSprite.color = initColor;
		}

		if (squirrel) {
			DogRB.AddForce((transform.position - squirrel.transform.position).normalized * -FollowSpeed * 2, ForceMode2D.Force);
		}

		if (trust > GrumpyThreshold
		&& State != DogState.Jumping
		&& Follow
		&& FollowDistance * FollowDistance < (transform.position - Player.transform.position).sqrMagnitude
		) {
			DogRB.AddForce((transform.position - Player.transform.position).normalized * -FollowSpeed, ForceMode2D.Force);
		}
	}

	public void SendCommand(float angle, float sqrDistance) {
		Debug.Log("Dog got command with angle " + angle);

		// TODO: branch depending on trust level

		float absAngle = Mathf.Abs(angle);

		if (absAngle > 70) {
			// TODO: walk closer to hand
		}

		if (absAngle < 70) {
			State = DogState.Jumping;
			timer = JumpDelay;
			if (Mathf.Abs(DogRB.velocity.x) > 0.1f)
				timer += JumpDelayIfStanding;

			queuedJumpAngle = angle + 90;

			float anglePercent = absAngle / 90f;
			float offsetEval = JumpOffsetCurve.Evaluate(anglePercent);

			queuedJumpAngle += offsetEval * JumpOffsetMul * Mathf.Sign(angle);

			dogAnimator.SetTrigger("Jump");
			// TODO: trigger landing animation on touching ground

		}

	}

	public void AddTrust(float trustChange) {

		bool wasSitting = trust < GrumpyThreshold;
		trust += trustChange;
		if (trust > 1) trust = 1;
		if (trust < 0) trust = 0;
		DogPlatfomerUIScript.SetTargetFill(trust);

		if (trust < GrumpyThreshold) {
			dogAnimator.SetBool("Sitting", true);
		} else if (wasSitting) {
			dogAnimator.SetBool("Sitting", false);
		}

	}

	private void UpdateLeash() {
		if (!Leash) return;

		float playerDistance = Vector3.Distance(transform.position, Player.transform.position);
		float playerAngle = Vector3.SignedAngle(Vector3.up, Player.transform.position - transform.position, Vector3.forward) + 90f;
		float t = playerDistance / LeashLength;

		float width = leashInitWidth * t;
		Vector2 size = new Vector2(width, Leash.size.y);
		Leash.size = size;
		// Vector3 pos = Leash.transform.position;
		Vector3 pos = transform.position + (Player.transform.position - transform.position) * 0.5f;
		Leash.transform.position = pos;
		Leash.transform.rotation = Quaternion.AngleAxis(playerAngle, Vector3.forward);
	}

	private void OnMouseDown() {
		Debug.Log("Petted dog");
		Player.Pet();

		if (panicTimer > 0)
			AddTrust(.7f - trust);

		if (State == DogState.Exhausted) {
			if (trust < DistractedThreshold)
				AddTrust(DistractedThreshold - trust + .01f);
			dogAnimator.SetBool("Sitting", false);
		}

		if (trust < GrumpyThreshold)
			AddTrust(.05f);
		else
			AddTrust(.1f);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Platform")) {
			float raycastX = Mathf.Abs(RaycastPos.x);
			if (!dogSprite.flipX) {
				raycastX = -raycastX;
			}

			RaycastHit2D hit = Physics2D.Raycast(
				// transform.position + 
				transform.TransformPoint(new Vector3(raycastX, RaycastPos.y, 0)),
				Vector2.down,
				RaycastDistance,
				RaycastMask
			);

			if (hit) {
				// IDEA: check angle of normal hit to only teleport onto horizontal surfaces
				// hit.normal
				Vector2 teleportPos = hit.point + Vector2.up * -TeleportOffset.y;
				// Debug.Log("Hit pos" + hit.point);
				// Debug.Log("Teleport from " + transform.position + " to " + teleportPos);
				// DogRB.MovePosition(
				// 	teleportPos
				// );

				DogRB.position = teleportPos;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Snake") && panicTimer < 0) {
			AddTrust(-.1f);
			panicTimer = PanicTime;
			dogAnimator.SetBool("Sitting", false);
		} else if (other.CompareTag("Squirrel") && State != DogState.Exhausted && State != DogState.Jumping) {
			// Debug.Log("touched squirrel");
			if (trust < DistractedThreshold) {
				fatigue = 1;
				squirrel = other.gameObject;
			}
		} else if (other.CompareTag("FallDamage")) {
			AddTrust(-.2f);
		}
	}

	// public void Distract(GameObject newSquirrel) {
	// 	if (trust < DistractedThreshold) {
	// 		fatigue = 1;
	// 		squirrel = newSquirrel;
	// 	}
	// }

	private void OnTriggerStay2D(Collider2D other) {
		if (trust < .3f && other.CompareTag("Squirrel")) {
			// distracted = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Squirrel")) {
			// distracted = false;
		}
	}
}
