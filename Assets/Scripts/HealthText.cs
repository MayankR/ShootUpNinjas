﻿using UnityEngine;
using System.Collections;

public class HealthText : MonoBehaviour {
	public GUIText healthText;
	int cur = 100;
	int reduceByQty = 0;
	bool reduce = false;

	// Use this for initialization
	void Start () {
		cur = 100;
		healthText.text = "Health: 100%";
		Vector3 ownPos = Camera.main.WorldToScreenPoint(transform.position);
		healthText.transform.position = new Vector2(ownPos.x / Screen.width, ownPos.y / Screen.height - 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (reduce && cur >= 0) {
			cur--;
			reduceByQty--;
			healthText.text = "Health: " + cur + "%";
			if (reduceByQty == 0) {
				reduce = false;
			}
		}
	}

	public void reduceBy(int num) {
		reduceByQty = num;
		reduce = true;
	}
}
