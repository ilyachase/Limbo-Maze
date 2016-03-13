using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab; // reference на prefab лабиринта
    public Circle circlePrefab;  // reference на prefab персонажа
	public Goal goalPrefab;
	public Bonus1 bonus1Prefab;
	public Bonus2 bonus2Prefab;
	public Bonus3 bonus3Prefab;
	public Counter counterPrefab;
	public Canvas canvasPrefab;
	public Circle circleInstance;  // reference на prefab персонажа
	public Goal goalInstance;
	public byte level = 1; // Текущий уровень

	private Maze mazeInstance; // instance лабиринта
	private const bool debug_c = true;

	void EndGame() {
		DeleteAll();
		Debug.Log("Game ended!");
	}

	void DeleteAll() {
		foreach (GameObject o in FindObjectsOfType<GameObject>())
			if ((o.name != "Game Manager") && (o.name != "Main Camera") && (o.name != "Maze"))
				Destroy(o);
	}

	// Точка вхождения игры
	void Update () {
		// По нажатию пробела запускаем игру
		if ((Input.GetKeyDown(KeyCode.Space)) && (GameObject.Find("Maze") == null))
			CreateAll(true);

		// По нажатию пробела запускаем игру
		if (Input.GetKeyDown(KeyCode.Tab) && debug_c)
			GoNextLevel();
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
	void CreateAll(bool generate = false, bool regenerate_current = false) {
		CreateMaze(generate, regenerate_current);
		CreateCircle();
		CreateGoal();
		CreateBonuses();
		if (!debug_c)
			CreateCounter();
		else {
			CreateBonus(3);
		}
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

	void CreateMaze(bool generate = false, bool regenerate_current = false) {
		if (generate) {
			mazeInstance = Instantiate(mazePrefab) as Maze;
			mazeInstance.GenerateLevels(regenerate_current);
		}
		else if (regenerate_current) {
			mazeInstance.GenerateLevels(regenerate_current, level);
		}

		mazeInstance.InstantiateMaze(level);
		mazeInstance.name = "Maze";
	}

	void CreateCounter() {
		var canvas = Instantiate(canvasPrefab) as Canvas;
		canvas.name = "Canvas";
	}

	// Переход на следующий уровень
	public void GoNextLevel() {
		level++;

		if (level == 9) {
			EndGame();
			return;
		}

		// Удаляем всё
		DeleteAll();

		// И пересоздаем
		CreateAll();

		// Выставляем камеру
		SetUpCam();
	}

	public void RestartLevel() {
		// Удаляем всё
		DeleteAll();

		// И пересоздаем
		CreateAll(false, true);

		// Выставляем камеру
		SetUpCam();
	}
}
