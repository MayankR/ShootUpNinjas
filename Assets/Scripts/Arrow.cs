using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	bool collided = false;
	bool destroy = false;
	int destroyTime = 80;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		Vector2 curVelocity = rb.velocity;
		if (curVelocity.magnitude > 0 && !collided) {
			if (curVelocity.x >= 0) {
				transform.eulerAngles = new Vector3 (0, 0, Mathf.Rad2Deg * Mathf.Atan (curVelocity.y / curVelocity.x));
			}
			else {
				transform.eulerAngles = new Vector3 (0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan (curVelocity.y / curVelocity.x));
			}
		}
		if (destroy) {
			destroyTime--;
			if (destroyTime == 0) {
				Destroy (gameObject);

			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		collided = true;
		Debug.Log ("Collided with " + coll.gameObject.name);
		string name = coll.gameObject.name;
		destroy = true;

	}
}