using System.Collections;
using UnityEngine;
using System.IO;
using System;

public static class MonoHelper {
	#region Graphics
	/// <summary>
	/// Textures the circle mask.
	/// </summary>
	/// <returns>The circle mask.</returns>
	/// <param name="h">The height.</param>
	/// <param name="w">The width.</param>
	/// <param name="r">The red component.</param>
	/// <param name="cx">Cx.</param>
	/// <param name="cy">Cy.</param>
	/// <param name="sourceTex">Source tex.</param>
	public static Texture2D TextureCircleMask(int h, int w, float r, float cx, float cy, Texture2D sourceTex) {
		Color[] c = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
		Texture2D b = new Texture2D(h, w);
		for (int i = 0; i < (h * w); i++) {
			int y = Mathf.FloorToInt(((float)i) / ((float)w));
			int x = Mathf.FloorToInt(((float)i - ((float)(y * w))));
			if (r * r >= (x - cx) * (x - cx) + (y - cy) * (y - cy)) {
				b.SetPixel(x, y, c[i]);
			} else {
				b.SetPixel(x, y, Color.clear);
			}
		}
		b.Apply();
		return b;
	}
	#endregion

	#region Code Mechanic
	public static IEnumerator DoSomeThingAfterFrame(int frameWait, UnityEngine.Events.UnityAction callback) {
		for (int i = 0; i < frameWait; i++)
			yield return null;

		if (callback != null)
			callback.Invoke();
	}
	public static IEnumerator DoSomeThing(float seconds, UnityEngine.Events.UnityAction callback) {
		yield return new WaitForSeconds(seconds);

		if (callback != null)
			callback.Invoke();
	}
	#endregion

	public static IEnumerator DownloadTexture(string url, Action<Texture2D> callback) {
		// Start a download of the given URL
		using (WWW www = new WWW(url)) {
			// Wait for download to complete
			yield return www;

			callback(www.texture);
		}
	}

	public static Texture2D LoadPNG(string filePath) {

		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath)) {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
		}
		return tex;
	}

	// First launch
	public static bool IsFirstLaunch(string name) {
		return (PlayerPrefs.GetInt(name + "_first_launch_pref_string", 1) == 1) ? true : false;
	}

	public static void SetIsFirstLaunchDone(string name) {
		PlayerPrefs.SetInt(name + "_first_launch_pref_string", 0);
	}

	public static IEnumerator DownloadFileCoroutine(string url, Action<string> successCallback, Action<string> failCallback) {
		WWW www = new WWW(url);

		yield return www;

		if (www.error != null) {
			if (failCallback != null)
				failCallback.Invoke(www.error);
		} else if (www.bytesDownloaded == 0) // Cheat as error
		  {
			if (failCallback != null)
				failCallback.Invoke("(DownloadFileCoroutine) File empty!");
		} else {
			if (successCallback != null)
				successCallback(www.text);
		}
	}

	public static Stream GenerateStreamFromString(string s) {
		var stream = new MemoryStream();
		var writer = new StreamWriter(stream);
		writer.Write(s);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}


	public static Vector2 GetCurve(Vector2 vec1, Vector2 vec2, Vector2 dir, float dirpower, float maxpower, float powermove, float gravity = 0.5f) {
		float finalGravitation = gravity;

		dirpower = Math.Min(dirpower, maxpower);
		float rate = (maxpower - dirpower) / maxpower;

		Vector2 v3 = vec2 + (dir.normalized * powermove);
		Vector2 v2 = vec1 - v3;
		Vector2 p1 = vec2 + (dir.normalized * powermove * (1 - rate));
		Vector2 p2 = v3 + (v2.normalized * finalGravitation * rate);
		return p1 + (p2 - p1) * rate * 0.5f;

	}


	public static Vector2[] GetCurvePath(Vector2 point1, Vector2 point3, Vector2 dir, int numberofPoint, float add) {
		float distance = Vector2.Distance(point1, point3);
		Vector2 point2 = point1 + (dir.normalized * distance);
		Vector2[] result = new Vector2[numberofPoint];
		Vector2 vec1 = point2 - point1;
		Vector2 vec2 = point3 - point2;
		Vector2 final1;
		Vector2 final2;
		float rate = 0f;
		for (int i = 0; i < numberofPoint; i++) {

			rate = add + i / 10f;
			final1 = point1 + (vec1 * rate);
			final2 = point2 + (vec2 * rate);
			result[i] = final1 + ((final2 - final1) * rate);
		}
		return result;
	}


	public static Vector2 GetCurvePoint(Vector2 point1, Vector2 point3, Vector2 dir, float rate) {
		float distance = Vector2.Distance(point1, point3);
		Vector2 point2 = point1 + (dir.normalized * distance);
		Vector2 result;
		Vector2 vec1 = point2 - point1;
		Vector2 vec2 = point3 - point2;
		Vector2 final1;
		Vector2 final2;
		final1 = point1 + (vec1 * rate);
		final2 = point2 + (vec2 * rate);
		result = final1 + ((final2 - final1) * rate);

		return result;
	}
}