using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
	public bool is_fading_out = false, is_fading_in = false;
	public float fadeTime;

	RectTransform tr;
	Image img;
	float t;

	void Awake() {
		tr = transform as RectTransform;
		img = GetComponent<Image>();

		transform.SetParent(GameObject.Find("Canvas").transform);
		tr.sizeDelta = new Vector2(Screen.width, Screen.height);
		img.color = Color.clear;
	}

	public void FadeOut() {
		if ((is_fading_out) || (img.color == Color.black))
			return;
		is_fading_out = true;
	}

	public void FadeIn() {
		is_fading_in = true;
	}

	public void SetFront() {
		transform.SetAsLastSibling(); // Ставим на передний план
	}

	void Update() {
		transform.SetAsLastSibling(); // Ставим на передний план
		if (is_fading_out) {
			if (img.color != Color.black) {
				img.color = Color.Lerp(Color.clear, Color.black, t);
				t += fadeTime;
			}
			else {
				is_fading_out = false;
				t = 0;
			}
			return;
		}

		if (is_fading_in) {
			GameObject.Find("Counter").GetComponent<Text>().color = Color.white;
			if (img.color != Color.clear) {
				img.color = Color.Lerp(Color.black, Color.clear, t);
				t += fadeTime;
			}
			else {
				is_fading_out = false;
				t = 0;
			}
		}
	}
}
