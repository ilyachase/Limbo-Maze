using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Counter : MonoBehaviour {
	private GameManager gmRef;
	private int mil = 0, sec = 0;
	private int cur_lvl_limit;

	// Use this for initialization
	void Start () {
		gmRef = GameObject.Find("Game Manager").GetComponent<GameManager>();

		// Вычисление лимита времени на текующий уровень
		cur_lvl_limit = gmRef.level * 3;
	}
	
	// Update is called once per frame
	void Update () {
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
		if (sec >= cur_lvl_limit)
			gmRef.RestartLevel();
	}
}
