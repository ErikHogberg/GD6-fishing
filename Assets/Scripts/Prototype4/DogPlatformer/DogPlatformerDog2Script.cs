using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogPlatformerDog2Script : MonoBehaviour {

	public DogPlatformerHandScript Player;

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
	private bool distracted = false;

	public float SpriteFlipThreshold = 0.01f;

	private float trust = .5f;

	public float LeashLength = 1f;

	public float JumpForce = 10;
	public float JumpDelay = .1f;
	public float JumpDelayIfStanding = .1f;
	private float queuedJumpAngle;
	public AnimationCurve JumpOffsetCurve;
	public float JumpOffsetMul = 1;
	public float JumpMaxHandDistance;

	void Start() {
		DogRB = GetComponent<Rigidbody2D>();
		dogSprite = GetComponent<SpriteRenderer>();
		dogAnimator = GetComponent<Animator>();
		initColor = dogSprite.color;
	}

	private void Update() {
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

		dogAnimator.SetFloat("MoveSpeed", DogRB.velocity.x);

		Vector2 delta = Player.transform.position - transform.position;
		// float sqrDistance = delta.sqrMagnitude;
		if (delta.y > LeashLength) {
			// TODO: lose trust when lifting dog off ground with leash
			// IDEA: make dog enter exhausted state
			// Debug.Log("Dog is hanging off leash");
			dogSprite.color = Color.red;
		} else if (Mathf.Abs(delta.x) > LeashLength) {
			// Debug.Log("Dog is pulled by leash");
			dogSprite.color = Color.yellow;
		} else {
			dogSprite.color = initColor;
		}

		if (!distracted
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

		if (absAngle < 90) {
			State = DogState.Jumping;
			timer = JumpDelay;
			if(Mathf.Abs(DogRB.velocity.x) > 0.1f) 
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
		trust += trustChange;
		if (trust > 1) trust = 1;
		if (trust < 0) trust = 0;
		DogPlatfomerUIScript.SetBarPercent(trust);
	}

	private void OnMouseDown() {
		Debug.Log("Petted dog");
		AddTrust(.1f);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Snake")) {
			AddTrust(-.1f);
			// TODO: make dog panic
		}
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (trust < .3f && other.CompareTag("Squirrel")) {
			distracted = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Squirrel")) {
			distracted = false;
		}
	}
}
