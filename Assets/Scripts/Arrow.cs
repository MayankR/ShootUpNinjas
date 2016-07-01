using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	bool collided = false;
	bool destroy = false;
	int destroyTime = 80;
	public AICannon aiCannon;
	public GameObject userCannonBase;
	bool givenMiss = false;
	public bool isEnemyArrow = false;

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
		float ownY = transform.position.y;
		float targetY = userCannonBase.transform.position.y;
		float tolerance = 0.3f;
		if (!givenMiss && isEnemyArrow) {
			if (ownY <= targetY + tolerance && ownY >= targetY - tolerance && curVelocity.y < 0) {
				aiCannon.newMiss (transform.position.x - userCannonBase.transform.position.x);
				givenMiss = true;
				Debug.Log ("Missed by: " + (transform.position.x - userCannonBase.transform.position.x));
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		collided = true;
		Debug.Log ("Collided with " + coll.gameObject.name);
		string name = coll.gameObject.name;
		destroy = true;
		makeNewEnemy ();
	}

	void makeNewEnemy() {
		aiCannon.newEnemy ();
	}
}