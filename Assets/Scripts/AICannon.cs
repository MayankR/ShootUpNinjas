﻿using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start () {
		stage = 0;
		endX = startX + Random.Range (0.5f, 140);
		endY = startY - Random.Range (0.5f, 140);
	}
	
	// Update is called once per frame
	void Update () {

		if (stage == 0) {
			startX = transform.position.x;
			startY = transform.position.y;
			curX = endX;
			curY = endY;
			stage = 1;
			moveFrames = (int)Random.Range (80, 130);
			moveFrames = 100;
			setStartTouch (startX, startY);
			updateTouch (endX, endY);
			endX = startX + Random.Range (0.5f, 140);
			endY = startY - Random.Range (0.5f, 140);
		} else if (stage == 1 && moveFrames > 0) {
			curX = curX + (endX - startX) / moveFrames;
			curY = curY + (endY - startY) / moveFrames;
			updateTouch (curX, curY);
			moveFrames--;
			if (moveFrames == 0) {
				stage = 2;
			}
		} else if (stage == 2) {
			endTouch (endX, endY);
			stage = 3;
			pauseFrames = (int)Random.Range (20, 40);
		} else if (stage == 3) {
			if (pauseFrames == 0) {
				newEnemy ();
				stage = 0;
			}
			pauseFrames--;
		}
	}

	void newEnemy() {
		float vertical = 1f;
		float horizontal = 0.3f;
		float xChange = Random.Range (-1*vertical, vertical);
		float yChange = Random.Range (-1*horizontal, horizontal);
		transform.position = new Vector2(transform.position.x + xChange, transform.position.y + yChange);
		Vector2 oldOwnCannonBase = ownCannonBase.transform.position;
		ownCannonBase.transform.position = new Vector2(oldOwnCannonBase.x + xChange, oldOwnCannonBase.y + yChange);
		Vector2 oldOwnHuman = ownHuman.transform.position;
		ownHuman.transform.position = new Vector2(oldOwnHuman.x + xChange, oldOwnHuman.y + yChange);
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
		Debug.Log (touchLength.magnitude);
		newArrow.velocity = dir.normalized * arrowSpeed * touchLength.magnitude/140;
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