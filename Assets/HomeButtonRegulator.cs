using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButtonRegulator : MonoBehaviour {
	GameObject button;
	Image buttonImage;

	// Use this for initialization
	void Start () {
		button = GameObject.Find ("HomeButton");
		buttonImage = button.GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowHomeButton() {
		buttonImage.enabled = true;
	}

	public void HideHomeButton() {
		buttonImage.enabled = false;
	}

	public void GoHome() {
		AudioSource[] sounds = GetComponents<AudioSource> ();
		AudioSource stopEchoSound = sounds[0];
		stopEchoSound.Play();
		RestoreDevicesAndLevel ();
	}

	public void RestoreDevicesAndLevel() {
		StartCoroutine (RestorationWrapper());
	}

	private IEnumerator RestorationWrapper() {
		yield return StartCoroutine(RestoreNest ());
		SceneManager.LoadScene(0);
	}

	IEnumerator RestoreNest() {
		return DevicesHelper.ChangeNestHelper ("60", "\"cool\"");
	}
}
