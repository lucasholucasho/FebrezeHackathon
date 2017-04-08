using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TakeHome : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")) {
			RestoreDevicesAndLevel ();
		}
	}

	public void RestoreDevicesAndLevel() {
		StartCoroutine (RestorationWrapper());
	}

	private IEnumerator RestorationWrapper() {
		yield return StartCoroutine(RestoreNest ());
		Application.LoadLevel (0);
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