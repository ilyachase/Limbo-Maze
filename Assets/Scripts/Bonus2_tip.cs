using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bonus2_tip : MonoBehaviour {
	bool need_show = false, whited = false;
	float wait = 0, t = 0, fadeTime = 0.02f;
	Text text;

	// Use this for initialization
	void Awake() {
		text = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update() {
		if (!need_show)
			return;

		if ((text.color != Color.white) && (!whited)) {
			text.color = Color.Lerp(Color.clear, Color.white, t);
			t += fadeTime;
			return;
		} else if (!whited) {
			whited = true;
			t = 0;
		}

		if ((wait < 5) && (text.color == Color.white)) {
			wait += Time.deltaTime;
			return;
		}

		if (text.color != Color.clear) {
			text.color = Color.Lerp(Color.white, Color.clear, t);
			t += fadeTime;
			return;
		}

		need_show = false;
	}

	public void Show() {
		need_show = true;
	}
}
