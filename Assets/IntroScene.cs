using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {

	void Start() {

	}

	void LateUpdate() {
		GvrViewer.Instance.UpdateState();
		if (GvrViewer.Instance.BackButtonPressed) {
			Application.Quit();
		}
	}

	public void ClickScene(int levelIndex) {
		SceneManager.LoadScene(levelIndex);
	}
}