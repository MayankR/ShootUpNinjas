using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	bool collided = false;
	bool destroy = false;
	int destroyTime = 80;
	public AICannon aiCannon;
	public GameObject aiCannonBase;
	public GameObject userCannonBase;
	bool givenMiss = false;
	public bool isEnemyArrow = false;
	int offScreenTime = 200;
	bool damageDone = false;

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
			}
		}
		Vector3 ownPos = Camera.main.WorldToScreenPoint (transform.position);
		if (ownPos.y <= 0 || ownPos.x <= 0 || ownPos.x >= Screen.width) {
			offScreenTime--;
			if (offScreenTime == 0) {
				if (isEnemyArrow) {
					Destroy (gameObject);
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("Collided with " + coll.gameObject.name);
		destroy = true;
		string name = coll.gameObject.name;
		if (!damageDone) {
			if (name == "head") {
				HealthText hText = aiCannonBase.GetComponent<HealthText> ();
				hText.reduceBy (50 + (int)Random.Range(-10, 10));
				damageDone = true;
			} else if (name == "body") {
				HealthText hText = aiCannonBase.GetComponent<HealthText> ();
				hText.reduceBy (40 + (int)Random.Range(-5, 5));
				damageDone = true;
			} else if (name == "leg1" || name == "leg2") {
				HealthText hText = aiCannonBase.GetComponent<HealthText> ();
				hText.reduceBy (20 + (int)Random.Range(-5, 5));
				damageDone = true;
			}
			else if (name == "headUser") {
				HealthText hText = userCannonBase.GetComponent<HealthText> ();
				hText.reduceBy (70);
				damageDone = true;
			} else if (name == "bodyUser") {
				HealthText hText = userCannonBase.GetComponent<HealthText> ();
				hText.reduceBy (50);
				damageDone = true;
			} else if (name == "leg1User" || name == "leg2User") {
				HealthText hText = userCannonBase.GetComponent<HealthText> ();
				hText.reduceBy (40);
				damageDone = true;
			}
		}
		collided = true;
	}

	void makeNewEnemy() {
		aiCannon.newEnemy ();
	}
}