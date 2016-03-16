using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {
	public bool is_fading_out = false;
	
	void Awake() {
		transform.SetParent(GameObject.Find("Canvas").transform);
		var tr = transform as RectTransform;
		tr.sizeDelta = new Vector2(Screen.width, Screen.height);
	}

	public void FadeOut() {

	}
}
