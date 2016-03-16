using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
	GameManager gmRef;
	Fader fRef;
	int mil = 0, sec = 0;
	int cur_lvl_limit;
	float slow_time = 0;
	bool skip = false;

	// Use this for initialization
	void Awake() {
		gmRef = GameObject.Find("Game Manager").GetComponent<GameManager>();
		fRef = GameObject.Find("Fader").GetComponent<Fader>();

		transform.SetParent(GameObject.Find("Canvas").transform);
		var tr = transform as RectTransform;
		tr.pivot = new Vector2(0, 0);
		tr.localScale = new Vector3(1, 1);
		tr.anchoredPosition3D = new Vector3(30, -70, 0);

		// Вычисление лимита времени на текующий уровень
		cur_lvl_limit = 1 + gmRef.level * 3;
	}
	
	// Update is called once per frame
	void Update () {
		if (slow_time > 0) {
			slow_time -= Time.deltaTime;
			skip = !skip;
			if (skip)
				return;
		}

		var counter = GetComponent<Text>();

		string mil_s = (mil < 10) ? "0" : "";
		mil_s += mil;

		if (mil < 60)
			mil++;
		else {
			mil = 0;
			sec++;
		}

		string sec_s = (sec < 10) ? "0" : "";
		sec_s += sec;

		counter.text = sec_s + ":" + mil_s;

		// Если лимит исчерпан, перезапускаем уровень
		if (sec >= cur_lvl_limit) {
			fRef.FadeOut();
			// Fade out scene
			while (fRef.is_fading_out)
				return;
			
			// Для 6-8 уровней не рестартим, а динамически изменяем лабиринт
			if (gmRef.level >= 6)
				gmRef.RestartLevel(true);
			else
				gmRef.RestartLevel();

			fRef.FadeIn();
		}
	}

	public void ActiveteBonus2() {
		slow_time = 5f;
	}
}
