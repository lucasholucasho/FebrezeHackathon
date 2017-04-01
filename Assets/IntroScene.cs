using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

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

	public void RestoreDevicesAndLevel(int levelIndex) {
		StartCoroutine (RestorationWrapper(levelIndex));
	}

	private IEnumerator RestorationWrapper(int levelIndex) {
		yield return StartCoroutine(RestoreNest ());
		yield return StartCoroutine(RestoreFebreze ());
		ClickScene (0);
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
}

