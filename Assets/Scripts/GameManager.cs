using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab; // reference на prefab лабиринта
    public Circle circlePrefab;  // reference на prefab персонажа
	public Goal goalPrefab;
	public Bonus1 bonus1Prefab;
	public Bonus2 bonus2Prefab;
	public Bonus3 bonus3Prefab;
	public Counter counterPrefab;
	public Circle circleInstance;  // reference на prefab персонажа
	public Goal goalInstance;
	public byte level = 1; // Текущий уровень
	public float introTextFadeTime;

	Maze mazeInstance; // instance лабиринта
	const bool debug_c = false;
	bool created = false, i1end = false, i2end = false, i3end = false, i4end = false, need_end = false, deleted = false;
	Fader fRef;
	Text i1, i2, i3, i4, outro;
	GameObject b1, b2, b3;
	float t = 0, wait = 0;

	void EndGame() {
		if (fRef.is_fading_out)
			return;
		
		if (!deleted) {
			DeleteAll();
			deleted = true;
		}

		if (outro.color != Color.white) {
			outro.color = Color.Lerp(Color.clear, Color.white, t);
			t += introTextFadeTime;
			return;
		}
	}

	void HideAllBonusesText() {
		b1.GetComponent<Image>().enabled = false;
		b2.GetComponent<Image>().enabled = false;
		b3.GetComponent<Image>().enabled = false;
		GameObject.Find("Bonuses").GetComponent<Text>().color = Color.clear;
		GameObject.Find("Bonus1_tip").GetComponent<Text>().color = Color.clear;
		GameObject.Find("Bonus2_tip").GetComponent<Text>().color = Color.clear;
		GameObject.Find("Bonus3_tip").GetComponent<Text>().color = Color.clear;
	}

	void Awake() {
		fRef = GameObject.Find("Fader").GetComponent<Fader>();
		i1 = GameObject.Find("Intro1").GetComponent<Text>();
		i2 = GameObject.Find("Intro2").GetComponent<Text>();
		i3 = GameObject.Find("Intro3").GetComponent<Text>();
		i4 = GameObject.Find("Intro4").GetComponent<Text>();
		outro = GameObject.Find("Outro").GetComponent<Text>();
		
		b1 = GameObject.Find("Bonus1_icon");
		b2 = GameObject.Find("Bonus2_icon");
		b3 = GameObject.Find("Bonus3_icon");

		HideAllBonusesText();
	}

	void DeleteAll(bool dynamically = false) {
		foreach (GameObject o in FindObjectsOfType<GameObject>())
			if (o.tag != "Protected") {
				if ((dynamically) && (o.name == "Circle")) {
					o.GetComponent<Circle>().DelBonuses();
					continue;
				}
				Destroy(o);
			}

		HideAllBonusesText();
	}

	// Точка вхождения игры
	void Update () {
		if (Input.GetKeyDown(KeyCode.Tab) && debug_c)
			GoNextLevel();

		if (Input.GetKeyDown(KeyCode.R) && debug_c)
			RestartLevel(true);

		if (need_end) {
			EndGame();
			return;
		}

		if (created)
			return;

		if (debug_c) {
			i1.color = Color.white;
			i2.color = Color.white;
			i3.color = Color.white;
			i4.color = Color.white;
			wait = 10;
		}

		// По очереди показываем строки интро
		if ((wait < 1) && (i1.color == Color.clear)) {
			wait += Time.deltaTime;
			return;
		}

		if (i1.color != Color.white) {
			i1.color = Color.Lerp(Color.clear, Color.white, t);
			t += introTextFadeTime;
			return;
		}
		else if (!i1end) {
			i1end = true;
			t = 0;
			wait = 0;
		}

		if ((wait < 1) && (i2.color == Color.clear)) {
			wait += Time.deltaTime;
			return;
		}

		if (i2.color != Color.white) {
			i2.color = Color.Lerp(Color.clear, Color.white, t);
			t += introTextFadeTime;
			return;
		}
		else if (!i2end) {
			i2end = true;
			t = 0;
			wait = 0;
		}

		if ((wait < 1) && (i3.color == Color.clear)) {
			wait += Time.deltaTime;
			return;
		}

		if (i3.color != Color.white) {
			i3.color = Color.Lerp(Color.clear, Color.white, t);
			t += introTextFadeTime;
			return;
		}
		else if (!i3end) {
			i3end = true;
			t = 0;
			wait = 0;
		}

		if ((wait < 1) && (i4.color == Color.clear)) {
			wait += Time.deltaTime;
			return;
		}

		if (i4.color != Color.white) {
			i4.color = Color.Lerp(Color.clear, Color.white, t);
			t += introTextFadeTime;
			return;
		}
		else if (!i4end) {
			i4end = true;
			t = 0;
			wait = 0;
		}

		if (wait < 3) {
			wait += Time.deltaTime;
			return;
		}

		fRef.fadeTime = 0.01f;
		fRef.FadeOut();
		if (fRef.is_fading_out)
			return;

		i1.color = Color.clear;
		i2.color = Color.clear;
		i3.color = Color.clear;
		i4.color = Color.clear;

		CreateAll(true);
		fRef.FadeIn();
		fRef.fadeTime = 0.05f;
		created = true;
	}

	void SetUpCam() {
		var cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		cam.transform.position = new Vector3(mazeInstance.width * 0.49f, mazeInstance.height * 0.5f, -10);
		cam.orthographicSize = mazeInstance.width * 0.58f;
	}

	void CreateCircle() {
		circleInstance = Instantiate(circlePrefab) as Circle;
		circleInstance.name = "Circle";
	}

	void CreateGoal() {
		goalInstance = Instantiate(goalPrefab) as Goal;
		goalInstance.name = "Goal";
	}

	void CreateBonuses() {
		// Если нужно, расставляем бонусы
		if (level >= 4) {
			CreateBonus(1, 5);
			if (level >= 5) {
				CreateBonus(1, 8);
				CreateBonus(2, 5);
			}
			if (level >= 6) {
				CreateBonus(1, 11);
				CreateBonus(2, 8);
				CreateBonus(3, 5);
			}
			if (level >= 7) {
				CreateBonus(2, 11);
				CreateBonus(3, 8);
			}
			if (level >= 8) {
				CreateBonus(3, 11);
			}
		}
	}

	// Запуск игры
	void CreateAll(bool generate = false, bool regenerate_current = false, bool dynamically = false) {
		CreateMaze(generate, regenerate_current, dynamically);
		if (!dynamically)
			CreateCircle();
		CreateGoal();
		CreateBonuses();
		CreateCounter();
		SetUpCam();
	}

	void CreateBonus(byte num, byte depth = 0) {
		if (num == 1) {
			var b1 = Instantiate(bonus1Prefab) as Bonus1;
			b1.InstantiateBonus(depth);
			b1.name = "Bonus1";
		}
		else if (num == 2) {
			var b2 = Instantiate(bonus2Prefab) as Bonus2;
			b2.InstantiateBonus(depth);
			b2.name = "Bonus2";
		}
		else {
			var b3 = Instantiate(bonus3Prefab) as Bonus3;
			b3.InstantiateBonus(depth);
			b3.name = "Bonus3";
		}
	}

	void CreateMaze(bool generate = false, bool regenerate_current = false, bool dynamically = false) {
		if (generate) {
			mazeInstance = Instantiate(mazePrefab) as Maze;
			mazeInstance.GenerateLevels(regenerate_current);
		}
		else if (regenerate_current) {
			mazeInstance.GenerateLevels(regenerate_current, level, dynamically);
		}

		mazeInstance.InstantiateMaze(level);
		mazeInstance.name = "Maze";
	}

	void CreateCounter() {
		var canvas = Instantiate(counterPrefab) as Counter;
		canvas.name = "Counter";
	}

	// Переход на следующий уровень
	public void GoNextLevel() {
		level++;

		if (level == 9) {
			fRef.FadeOut();
			need_end = true;
			return;
		}

		// Удаляем всё
		DeleteAll();

		// И пересоздаем
		CreateAll();

		// Выставляем камеру
		SetUpCam();

		fRef.FadeIn();
	}

	public void RestartLevel(bool dynamically = false) {
		if (dynamically)
			circleInstance.Block();

		// Удаляем всё
		DeleteAll(dynamically);

		// И пересоздаем
		CreateAll(false, true, dynamically);

		if (dynamically)
			circleInstance.Unblock();

		// Выставляем камеру
		SetUpCam();
	}
}
