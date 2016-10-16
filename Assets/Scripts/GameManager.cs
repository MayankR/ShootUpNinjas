using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject playButton;
	public GameObject replayButton;
	public GameObject aiCannonBase;
	public GameObject userCannonBase;
	public AICannon aiCannon;
	public Cannon cannon;
	int gameState = 0;

	// Use this for initialization
	void Start () {
		gameState = 0;
		replayButton.transform.localScale = new Vector3 (0, 0, 0);
		replayButton.transform.position = new Vector3 (200, 200, -20);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState == 0 || gameState == 2) {
			if (Input.touchCount > 0) {
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
					// RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
					if(hitInfo)
					{
						Debug.Log( hitInfo.transform.gameObject.name );
						string name = hitInfo.transform.gameObject.name;
						if (name == "playButton" || name == "replayButton") {
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
					string name = hitInfo.transform.gameObject.name;
					if (name == "playButton" || name == "replayButton") {
						updateState (1);
					}
				}
			}
		}
	}

	void updateState(int n) {
		if (gameState == 0) { 	//Game not started yet
			if (n == 1) {
				playButton.transform.localScale = new Vector3 (0, 0, 0);
				playButton.transform.position = new Vector3 (200, 200, -20);
				aiCannon.updateState (1);
				cannon.updateState (1);
			}
		}
		else if (gameState == 2) {	//User lost last time
			if (n == 1) {
				replayButton.transform.localScale = new Vector3 (0, 0, 0);
				replayButton.transform.position = new Vector3 (200, 200, -20);
				aiCannon.updateState (1);
				cannon.updateState (1);
				HealthText hText = userCannonBase.GetComponent<HealthText> ();
				hText.resetHealth ();
				hText = aiCannonBase.GetComponent<HealthText> ();
				hText.resetHealth ();
			}
		}
	}

	public void playerDead() {
		aiCannon.updateState (2);
		cannon.updateState (2);
		gameState = 2;
		replayButton.transform.localScale = new Vector3 (0.2461227f, 0.2461227f, 0.2461227f);
		replayButton.transform.position = new Vector3 (0, -1.2f, 0);
		manageScore ();
	}

	void manageScore() {
		int curScore = aiCannon.getScore ();
		Debug.Log ("Current Score: " + curScore);
		int highestScore = SaveData.loadScore ();
		Debug.Log ("Highest Score: " + highestScore);
		if (curScore <= highestScore) {
			return;
		} else {
			SaveData.storeScore (curScore);
		}

	}
}
