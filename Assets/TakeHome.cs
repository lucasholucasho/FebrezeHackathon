﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TakeHome : MonoBehaviour {

	bool insideOtherButton = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1") && insideOtherButton == false) {
			RestoreDevicesAndLevel ();
		}
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

	public void OnMouseEnter() {
		insideOtherButton = true;
	}

	public void OnMouseExit() {
		insideOtherButton = false;
	}
}