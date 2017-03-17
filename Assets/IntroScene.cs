using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		Application.LoadLevel (levelIndex);
	}
}

