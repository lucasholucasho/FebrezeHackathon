using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class PhilzRegulator : MonoBehaviour, IGvrGazeResponder {
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
		SetGazedAt(false);
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

	public void SetGazedAt(bool gazedAt) {
		GetComponent<Renderer>().material.color = gazedAt ? Color.green : Color.red;
	}

	public void ToggleVRMode() {
		GvrViewer.Instance.VRModeEnabled = !GvrViewer.Instance.VRModeEnabled;
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

	#region IGvrGazeResponder implementation
	/// Called when the user is looking on a GameObject with this script,
	/// as long as it is set to an appropriate layer (see GvrGaze).
	public void OnGazeEnter() {
		SetGazedAt(true);
	}

	/// Called when the user stops looking on the GameObject, after OnGazeEnter
	/// was already called.
	public void OnGazeExit() {
		SetGazedAt(false);
	}

	/// Called when the viewer's trigger is used, between OnGazeEnter and OnGazeExit.
	public void OnGazeTrigger() {
		StartCoroutine(RestoreNest());
		StartCoroutine (RestoreFebreze());
	}

	IEnumerator RestoreFebreze() {
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("[{\"DeviceAction\": \"led_mode=1\" }, {\"DeviceAction\": \"led_color=0,11,4,4,4\" }]");
		UnityWebRequest www = UnityWebRequest.Put("https://na-hackathon-api.arrayent.io:443/v3/devices/50331667", myData);
		www.SetRequestHeader("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGllbnRfaWQiOiI0ZmNhNzZlMC0wMTM2LTExZTctOTIwNy1iNWMzYjY2M2Y2YTQiLCJlbnZpcm9ubWVudF9pZCI6Ijk0OGUyY2YwLWZkNTItMTFlNi1hZTQ2LTVmYzI0MDQyYTg1MyIsInVzZXJfaWQiOiI5MDAwMTAyIiwic2NvcGVzIjoie30iLCJncmFudF90eXBlIjoiYXV0aG9yaXphdGlvbl9jb2RlIiwiaWF0IjoxNDg4Njc0NDM2LCJleHAiOjE0ODk4ODQwMzZ9.y9Wwtsnk7zNWsp5V1Bd9HlKWb3yObatB1plKl_xNvutQbO_69LAWLbOcdqHCflacbbij940Q0wY4gtSgCdW2WQ");
		www.SetRequestHeader ("Content-Type", "application/json");

		yield return www.Send();

		if(www.responseCode == 200) {
			Debug.Log ("Color changed to purple");
		}
		else {
			Debug.Log ("Color could not change");
		}
	}

	IEnumerator RestoreNest() {
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("{\"target_temperature_f\": 60, \"hvac_mode\": \"cool\"}");
		UnityWebRequest www = UnityWebRequest.Put("https://developer-api.nest.com/devices/thermostats/Hluc73AxK2_cSWE0pOzopcLlBZ2DDRPz?auth=c.v4Bx7T27sQbK8UAYBqTK0Radej6QnhhjlxExZka697XEAD73xFiNJ5sNof7F1WtXcGwDmfydLvl0WHW0DcaIFzuwucBXrK3QlXkvBM8Bh8Sa1jWc3r8pslXHSCfOJuVRTparirKeaukHsiSR", myData);

		yield return www.Send();

		if(www.responseCode == 200) {
			Debug.Log ("Temparature changed to 60 and hvac mode changed to cool");
		}
		else {
			Debug.Log ("Temperature and hvac mode could not change");
		}
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
		byte[] myData = System.Text.Encoding.UTF8.GetBytes("{\"target_temperature_f\": 75, \"hvac_mode\": \"cool\"}");
		UnityWebRequest www = UnityWebRequest.Put("https://developer-api.nest.com/devices/thermostats/Hluc73AxK2_cSWE0pOzopcLlBZ2DDRPz?auth=c.v4Bx7T27sQbK8UAYBqTK0Radej6QnhhjlxExZka697XEAD73xFiNJ5sNof7F1WtXcGwDmfydLvl0WHW0DcaIFzuwucBXrK3QlXkvBM8Bh8Sa1jWc3r8pslXHSCfOJuVRTparirKeaukHsiSR", myData);

		yield return www.Send();

		if(www.responseCode == 200) {
			Debug.Log ("Temparature changed to 75 and hvac mode changed to cool");
		}
		else {
			Debug.Log ("Temperature and hvac mode could not change");
		}
	}

	#endregion
}