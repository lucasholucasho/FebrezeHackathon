using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BeachRegulator : MonoBehaviour {

	void Start() {
		System.Threading.Thread.Sleep(3000);
		AudioSource[] sounds = GetComponents<AudioSource> ();
		AudioSource startBeachSounds = sounds[0];
		startBeachSounds.Play();
		ChangeNest();
	}

	void LateUpdate() {
		GvrViewer.Instance.UpdateState();
		if (GvrViewer.Instance.BackButtonPressed) {
			Application.Quit();
		}
	}

	public void ToggleDistortionCorrection() {
		switch(GvrViewer.Instance.DistortionCorrection) {
		case GvrViewer.DistortionCorrectionMethod.Unity:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Native;
			break;
		case GvrViewer.DistortionCorrectionMethod.Native:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.None;
			break;
		case GvrViewer.DistortionCorrectionMethod.None:
		default:
			GvrViewer.Instance.DistortionCorrection = GvrViewer.DistortionCorrectionMethod.Unity;
			break;
		}
	}

	public void ToggleDirectRender() {
		GvrViewer.Controller.directRender = !GvrViewer.Controller.directRender;
	}

	public void ChangeNest() {
		StartCoroutine(ChangeNestHelper());
	}

	IEnumerator ChangeNestHelper() {
		return DevicesHelper.ChangeNestHelper ("79", "\"heat\"");
	}
}