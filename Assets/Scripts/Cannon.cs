using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	public Rigidbody2D arrow;
	public float gravity = 2.5f;
	public Animator explosion;
	public bool directionRight = true;
	public bool user = true;
	public GameManager gameManager;
	float startX, startY;
	float endX, endY;
	float arrowSpeed = 18.0f;
	float minAngle = -15, maxAngle = 85;
	int explodeFrames = 0;
	int gameState = 0;
	bool startedTouch = false;

	public void updateState(int n) {
		gameState = n;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == 1) {
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					setStartTouch (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
				} else if (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetMouseButton (0)) {
					updateTouch (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
				} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					endTouch (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
				}
			} else if (Input.GetMouseButton (0)) {
				if (Input.GetMouseButtonDown (0)) {
					setStartTouch (Input.mousePosition.x, Input.mousePosition.y);
				} else if (Input.GetMouseButton (0)) {
					updateTouch (Input.mousePosition.x, Input.mousePosition.y);
				} else if (Input.GetMouseButtonUp (0)) {
					endTouch (Input.mousePosition.x, Input.mousePosition.y);
				}
			} else if (Input.GetMouseButtonUp (0)) {
				endTouch (Input.mousePosition.x, Input.mousePosition.y);
			}


		}
		if (explodeFrames > 0) {
			explodeFrames--;
			if (explodeFrames <= 0) {
				explosion.SetBool ("doExplode", false); 
			}
		}
	}

	void endTouch(float x, float y) {
		if (!startedTouch) {
			return;
		}
		endX = x;
		endY = y;
		Quaternion ownRot = transform.rotation;
		Vector3 ownOld = ownRot.eulerAngles;
		ownRot.eulerAngles = new Vector3 (ownOld.x, ownOld.y, ownOld.z + 90);

		float angle = Mathf.Deg2Rad * ownRot.eulerAngles.z;
		Vector3 alongCannon = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);

		Rigidbody2D newArrow = Instantiate(arrow, transform.position + alongCannon*2, ownRot) as Rigidbody2D;
		newArrow.gravityScale = gravity;
		newArrow.transform.localScale = new Vector3 (0.1f, 0.2f, 1.0f);
		Vector2 dir = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));

		Vector2 touchLength = new Vector2 (endX - startX, endY - startY);
//		Debug.Log (touchLength.magnitude);
		newArrow.velocity = dir.normalized * arrowSpeed * touchLength.magnitude/140;
		startedTouch = false;
	}

	void setStartTouch (float x, float y) {
		startedTouch = true;
		startX = x;
		startY = y;
	}

	void updateTouch(float x, float y) {
		float curX = x;
		float curY = y;
		float angle = Vector2.Angle (new Vector2 (1, 0), new Vector2 (startX - curX, startY - curY));
		if (curX == startX && curY == startY) {
			angle = 0;
		}
		if (curY > startY) {
			angle = angle * -1;
		}
		if (angle < minAngle) {
			angle = minAngle;
		} else if (angle > maxAngle) {
			angle = maxAngle;
		}
		transform.eulerAngles = new Vector3 (0, 0, -90 + angle);
	}

	//User is dead. Called by health text.
	public void dead() {
		Debug.Log ("Dead called");
		explosion.transform.position = transform.position;
		explosion.SetBool ("doExplode", true);
		explodeFrames = 50;
		gameManager.playerDead ();
	}
}
