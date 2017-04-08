using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BeachRegulator : MonoBehaviour {
	private Vector3 startingPosition;

	void Start() {
		startingPosition = transform.localPosition;
		System.Threading.Thread.Sleep(3000);
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
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("{\"target_temperature_f\": 79, \"hvac_mode\": \"heat\"}");
		UnityWebRequest www = UnityWebRequest.Put("https://developer-api.nest.com/devices/thermostats/Hluc73AxK2_cSWE0pOzopcLlBZ2DDRPz?auth=c.v4Bx7T27sQbK8UAYBqTK0Radej6QnhhjlxExZka697XEAD73xFiNJ5sNof7F1WtXcGwDmfydLvl0WHW0DcaIFzuwucBXrK3QlXkvBM8Bh8Sa1jWc3r8pslXHSCfOJuVRTparirKeaukHsiSR", myData);

		yield return www.Send();

		if(www.responseCode == 200) {
			Debug.Log ("Temparature changed to 79 and hvac mode changed to heat");
		}
		else {
			Debug.Log ("Temperature and hvac mode could not change");
		}
	}
}