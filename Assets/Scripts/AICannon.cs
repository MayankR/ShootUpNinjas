using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AICannon : MonoBehaviour {
	public GameObject userCannonBase;
	public GameObject ownCannonBase;
	public GameObject ownHuman;
	float startX, startY;
	float curX, curY;
	float endX, endY;
	public Rigidbody2D arrow;
	float arrowSpeed = 18.0f;
	public bool directionRight = false;
	float minAngle = -15, maxAngle = 115;
	int stage = 0;
	int moveFrames = 100, pauseFrames = 30;
	float right, top, bottom;
	List<float> missList;

	// Use this for initialization
	void Start () {
		stage = 0;
		startX = transform.position.x;
		startY = transform.position.y;
		endX = startX + Random.Range (0.5f, 140);
		endY = startY - Random.Range (0.5f, 140);
		right = Camera.main.ScreenToWorldPoint (new Vector3(Screen.width, 0, 0)).x;
		top = Camera.main.ScreenToWorldPoint (new Vector3(0, Screen.height, 0)).y;
		bottom = Camera.main.ScreenToWorldPoint (new Vector3(0, 0, 0)).y;
		missList = new List<float> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (stage == 0) {
			curX = endX;
			curY = endY;
			stage = 1;
			moveFrames = (int)Random.Range (80, 130);
			moveFrames = 100;
//			Debug.Log ("Start from: " + startX + " " + startY);
			setStartTouch (startX, startY);
//			Debug.Log ("First update: " + endX + " " + endY);
			updateTouch (endX, endY);
			if (missList [missList.Count - 1] > 0) {
				endX = endX + Random.Range (-5, 50);
			}
			else {
				endX = endX + Random.Range (-50, 5);
			}
		} else if (stage == 1 && moveFrames > 0) {
			curX = curX + (endX - startX) / moveFrames;
			curY = curY + (endY - startY) / moveFrames;
//			Debug.Log ("Update: " + curX + " " + curY);
			updateTouch (curX, curY);
			moveFrames--;
			if (moveFrames == 0) {
				stage = 2;
			}
		} else if (stage == 2) {
//			Debug.Log ("End from: " + startX + " " + startY + " at: " + endX + " " + endY);
			endTouch (endX, endY);
			stage = 3;
			pauseFrames = (int)Random.Range (20, 40);
		} else if (stage == 3) {
			if (pauseFrames == 0) {
				stage = 0;
			}
			pauseFrames--;
		}
	}

	public void newEnemy() {
		float boundary = 0.5f;
		float verticalMax = (top - transform.position.y) * boundary;
		float verticalMin = (bottom - transform.position.y) * boundary;
		float horizontalMax = (right - transform.position.x) * boundary;
		float horizontalMin = (right*0.5f - transform.position.x) * boundary;
		Debug.Log (horizontalMin + " " + horizontalMax + " " + right);
		float yChange = Random.Range (verticalMin, verticalMax);
		float xChange = Random.Range (horizontalMin, horizontalMax);
		transform.position = new Vector2(transform.position.x + xChange, transform.position.y + yChange);
		Vector2 oldOwnCannonBase = ownCannonBase.transform.position;
		ownCannonBase.transform.position = new Vector2(oldOwnCannonBase.x + xChange, oldOwnCannonBase.y + yChange);
		Vector2 oldOwnHuman = ownHuman.transform.position;
		ownHuman.transform.position = new Vector2(oldOwnHuman.x + xChange, oldOwnHuman.y + yChange);
	}

	public void newMiss(float amount) {
		missList.Add (amount);
	}

	void endTouch(float x, float y) {
		endX = x;
		endY = y;
		Quaternion ownRot = transform.rotation;
		Vector3 ownOld = ownRot.eulerAngles;
		ownRot.eulerAngles = new Vector3 (ownOld.x, ownOld.y, ownOld.z - 90);
		float angle = Mathf.Deg2Rad * ownRot.eulerAngles.z;
		Vector3 alongCannon = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);

		Rigidbody2D newArrow = Instantiate(arrow, transform.position - alongCannon*2, ownRot) as Rigidbody2D;
		newArrow.transform.localScale = new Vector3 (0.1f, 0.2f, 1.0f);
		Vector2 dir = new Vector2 (startX - endX, startY - endY);

		Vector2 touchLength = new Vector2 (endX - startX, endY - startY);
		newArrow.velocity = dir.normalized * arrowSpeed * touchLength.magnitude/140;

		Arrow myArrow = newArrow.GetComponent<Arrow>();
		myArrow.isEnemyArrow = true;
	}

	void setStartTouch (float x, float y) {
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
		if (angle < 65 && angle > 0) {
			angle = 65;
		} else if (angle > -165 && angle < 0) {
			angle = -165;
		}
		transform.eulerAngles = new Vector3 (0, 0, -90 + angle);
	}
}