using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class BeachRegulator : MonoBehaviour {
	private Vector3 startingPosition;
	private static int GREEN = 1;
	private static int AQUA = 2;
	private static int EMERALD = 5;
	private static int PURPLE = 11;
	int[] colors = {GREEN, AQUA, EMERALD, PURPLE};
	private static int initialTargetTemperature = 0;
	private static string initialHVACMode = "";
	private int startIndex = 0;
	void Start() {
		startingPosition = transform.localPosition;
		System.Threading.Thread.Sleep(3000);
		ChangeFebreze();
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

	public void ChangeFebreze() {
		StartCoroutine(ChangeColorHelper());
	}

	IEnumerator ChangeColorHelper() {
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("[{\"DeviceAction\": \"led_mode=1\" }, {\"DeviceAction\": \"led_color=0,"+ colors[startIndex] + ",4,4,4\" }]");
		UnityWebRequest www = UnityWebRequest.Put("https://na-hackathon-api.arrayent.io:443/v3/devices/50331667", myData);
		www.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRfaWQiOiI0ZmNhNzZlMC0wMTM2LTExZTctOTIwNy1iNWMzYjY2M2Y2YTQiLCJlbnZpcm9ubWVudF9pZCI6Ijk0OGUyY2YwLWZkNTItMTFlNi1hZTQ2LTVmYzI0MDQyYTg1MyIsInVzZXJfaWQiOiI5MDAwMTAyIiwic2NvcGVzIjoie30iLCJncmFudF90eXBlIjoiYXV0aG9yaXphdGlvbl9jb2RlIiwiaWF0IjoxNDg4Njc0NDM2LCJleHAiOjE0ODk4ODQwMzZ9.y9Wwtsnk7zNWsp5V1Bd9HlKWb3yObatB1plKl_xNvutQbO_69LAWLbOcdqHCflacbbij940Q0wY4gtSgCdW2WQ");
		www.SetRequestHeader ("Content-Type", "application/json");

		yield return www.Send();

		if(www.responseCode == 200) {
			Debug.Log ("Color changed to " + colors [startIndex]);
			startIndex = (startIndex + 1) % colors.Length;
		}
		else {
			Debug.Log ("Color could not change");
		}
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