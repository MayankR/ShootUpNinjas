using UnityEngine;
using System.Collections;

public class HealthText : MonoBehaviour {
	public GUIText healthText;

	// Use this for initialization
	void Start () {
		healthText.text = "Health: 100%";
		Vector3 ownPos = Camera.main.WorldToScreenPoint(transform.position);
		healthText.transform.position = new Vector2(ownPos.x / Screen.width, ownPos.y / Screen.height - 0.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
