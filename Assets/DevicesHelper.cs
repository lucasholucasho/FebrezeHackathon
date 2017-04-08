using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DevicesHelper : MonoBehaviour {
	public static IEnumerator ChangeNestHelper(string temperature, string hvacMode) {
		var requestBody = string.Format ("{0}\"target_temperature_f\": {1}, \"hvac_mode\": {2}{3}", "{", temperature, hvacMode, "}");
		byte[] myData = System.Text.Encoding.UTF8.GetBytes(requestBody);
		UnityWebRequest www = UnityWebRequest.Put("https://developer-api.nest.com/devices/thermostats/Hluc73AxK2_cSWE0pOzopcLlBZ2DDRPz?auth=c.v4Bx7T27sQbK8UAYBqTK0Radej6QnhhjlxExZka697XEAD73xFiNJ5sNof7F1WtXcGwDmfydLvl0WHW0DcaIFzuwucBXrK3QlXkvBM8Bh8Sa1jWc3r8pslXHSCfOJuVRTparirKeaukHsiSR", myData);

		yield return www.Send();

		if(www.responseCode == 200) {
			Debug.Log (string.Format("Temparature changed to {0} and hvac mode changed to {1}", temperature, hvacMode));
		}
		else {
			Debug.Log ("Temperature and hvac mode could not change");
		}

	}
}
