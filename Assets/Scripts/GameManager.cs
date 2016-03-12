using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab; // reference на prefab лабиринта
    public Circle circlePrefab;  // reference на prefab персонажа
	public Goal goalPrefab;
	public Bonus1 bonus1Prefab;
	public Bonus2 bonus2Prefab;
	public Bonus3 bonus3Prefab;

	private Maze mazeInstance; // instance лабиринта
	public Circle circleInstance;  // reference на prefab персонажа
	public Goal goalInstance;
	private byte level = 1; // Текущий уровень

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
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartGame();
		}

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

	// Запуск игры
	void StartGame() {
		// Если лабиринта ещё нет, инстанциируем его
		if (GameObject.Find("Maze") == null) {
            // Создаем лабиринт
			mazeInstance = Instantiate(mazePrefab) as Maze;
			mazeInstance.GenerateLevels();
			mazeInstance.InstantiateMaze(level);
			mazeInstance.name = "Maze";

			// Создаем персонажа
			CreateCircle();

			// Создаем цель
			CreateGoal();

			// Выставляем камеру
			SetUpCam();
        }
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

	void CreateMaze(byte level) {
		mazeInstance.InstantiateMaze(level);
		mazeInstance.name = "Maze";
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
		CreateMaze(level);
		CreateCircle();
		CreateGoal();

		// Если нужно, расставляем бонусы
		if (level >= 4) {
			CreateBonus(1, 5);
			if (level >= 5) {
				CreateBonus(1, 8);
				CreateBonus(2, 5);
			}
			if (level >= 6) {
				CreateBonus(1, 8);
				CreateBonus(2, 8);
				CreateBonus(3, 5);
			}
			if (level > 6) {
				CreateBonus(3, 8);
			}
		}

		// Выставляем камеру
		SetUpCam();
	}
}
