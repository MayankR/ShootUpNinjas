using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {
	float startX, startY;
	float endX, endY;
	public Rigidbody2D arrow;
	float arrowSpeed = 18.0f;
	public bool directionRight = true;
	public bool user = true;
	float minAngle = -15, maxAngle = 85;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				setStartTouch (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
			} else if (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetMouseButton(0)) {
				updateTouch (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				endTouch (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y);
			}
		}
		else if (Input.GetMouseButton (0)) {
			if (Input.GetMouseButtonDown (0)) {
				setStartTouch (Input.mousePosition.x, Input.mousePosition.y);
			} else if (Input.GetMouseButton(0)) {
				updateTouch (Input.mousePosition.x, Input.mousePosition.y);
			} else if (Input.GetMouseButtonUp(0)) {
				endTouch (Input.mousePosition.x, Input.mousePosition.y);
			}
		}else if (Input.GetMouseButtonUp(0)) {
			endTouch (Input.mousePosition.x, Input.mousePosition.y);
		}
	}

	void endTouch(float x, float y) {
		endX = x;
		endY = y;
		Quaternion ownRot = transform.rotation;
		Vector3 ownOld = ownRot.eulerAngles;
		ownRot.eulerAngles = new Vector3 (ownOld.x, ownOld.y, ownOld.z + 90);

		float angle = Mathf.Deg2Rad * ownRot.eulerAngles.z;
		Vector3 alongCannon = new Vector3 (Mathf.Cos (angle), Mathf.Sin (angle), 0);

		Rigidbody2D newArrow = Instantiate(arrow, transform.position + alongCannon*2, ownRot) as Rigidbody2D;
		newArrow.transform.localScale = new Vector3 (0.1f, 0.2f, 1.0f);
		Vector2 dir = new Vector2 (Mathf.Cos (angle), Mathf.Sin (angle));

		Vector2 touchLength = new Vector2 (endX - startX, endY - startY);
//		Debug.Log (touchLength.magnitude);
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
		if (angle < minAngle) {
			angle = minAngle;
		} else if (angle > maxAngle) {
			angle = maxAngle;
		}
		transform.eulerAngles = new Vector3 (0, 0, -90 + angle);
	}
}
