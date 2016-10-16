using UnityEngine;
using System.Collections;

public class HealthText : MonoBehaviour {
	public GUIText healthText;
	int cur = 100;
	int reduceByQty = 0;
	bool reduce = false;
	public AICannon aiCannon;
	public Cannon cannon;
	public bool user = false;

	// Use this for initialization
	void Start () {
		cur = 100;
		healthText.text = "Health: 100%";
		Vector3 ownPos = Camera.main.WorldToScreenPoint(transform.position);
		healthText.transform.position = new Vector2(ownPos.x / Screen.width, ownPos.y / Screen.height - 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (reduce && cur > 0) {
			cur--;
			reduceByQty--;
			healthText.text = "Health: " + cur + "%";
			if (reduceByQty == 0) {
				reduce = false;
			}
			if (cur == 0) {
				makeNewEnemy ();
			}
		}
	}

	public void reduceBy(int num) {
		reduceByQty = num;
		reduce = true;
	}

	//Called when current health text value goes to 0
	void makeNewEnemy() {
		if (user) {
			cannon.dead ();
		} else {
			aiCannon.newEnemy ();
		}
	}

	public void resetHealth() {
		cur = 100;
		reduce = false;
		Start ();
	}
}