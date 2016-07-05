using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject playButton;
	public AICannon aiCannon;
	public Cannon cannon;
	int gameState = 0;

	// Use this for initialization
	void Start () {
		gameState = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == 0) {
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
					// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
					if(hitInfo)
					{
						Debug.Log( hitInfo.transform.gameObject.name );
						if (hitInfo.transform.gameObject.name == "playButton") {
							updateState (1);
						}
					}
				}
			}
			else if (Input.GetMouseButtonUp (0)) {
				Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(pos), Vector2.zero);
				// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
				if(hitInfo)
				{
					Debug.Log( hitInfo.transform.gameObject.name );
					if (hitInfo.transform.gameObject.name == "playButton") {
						updateState (1);
					}
				}
			}
		}
	}

	void updateState(int n) {
		if (gameState == 0) {
			if (n == 1) {
				playButton.transform.localScale = new Vector3 (0, 0, 0);
				playButton.transform.position = new Vector3 (200, 200, -20);
				aiCannon.updateState (1);
				cannon.updateState (1);
			}
		}
	}
}
