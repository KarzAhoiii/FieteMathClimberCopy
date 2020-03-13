using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

public class Utils : MonoBehaviour {

	public static Vector2 getScreenSize () {
		float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		Vector2 size = new Vector2(worldScreenWidth, worldScreenHeight);

		return size;
	}

	public static Vector3 getTouchPos (Vector3 coords, Camera camera) {
		float cameraZPos = -camera.transform.position.z;
		Vector3 tempCoords = new Vector3(coords.x, coords.y, cameraZPos);
		Vector3 worlPoint = camera.ScreenToWorldPoint(tempCoords);
		Vector2 touchPos = new Vector3(worlPoint.x, worlPoint.y);
		return touchPos;
	}
    
    public static Vector2 getFrustrumSize (Camera camera) {
        var frustumHeight = Mathf.Abs(2.0f * camera.transform.position.z * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad));
        var frustumWidth = Mathf.Abs(frustumHeight * camera.aspect);
        Vector2 frustrumSize = new Vector2(frustumWidth, frustumHeight);
        return frustrumSize;
    }

	public static float getPercent (float total, float part) {
		return (100 / total) * part;
        
	}

	public static float getSizeFromPercent (float percent, float total) {
		return (total / 100) * percent;
	}

	public static Color32 HexToColor(string hex) {
	    var r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
	    var g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
	    var b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
	    return new Color32(r,g,b, 255);
	}

	public static float convertRange(
		float originalStart, float originalEnd, // original range
		float newStart, float newEnd, // desired range
		float value) // value to convert
	{
		float scale = (float)(newEnd - newStart) / (originalEnd - originalStart);
		return (float)(newStart + ((value - originalStart) * scale));
	}

	public static bool checkInternetConnection() {
		bool isConnected = false;
		HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://google.com");
		try {
			using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse()) {
				bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
				if (isSuccess) {
					using (StreamReader reader = new StreamReader(resp.GetResponseStream())) {

						isConnected = true;
					}
				}
			}
		} catch {
			isConnected = false;
		}
		return isConnected;
	}
}