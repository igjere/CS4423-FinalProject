using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UtilityBehaviors : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown("r")){//reload scene, for testing purposes
			ItemDatabase.Instance.ResetAvailableItems();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		 if (Input.GetKeyDown(KeyCode.Escape)){//reload scene, for testing purposes
		 	ItemDatabase.Instance.ResetAvailableItems();
			SceneManager.LoadScene("MainMenu");
		}
	}
}
