using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab; // reference на prefab лабиринта
    public Circle circlePrefab;  // reference на prefab персонажа
	public Goal goalPrefab;

    private Maze mazeInstance; // instance лабиринта
	public Circle circleInstance;  // reference на prefab персонажа
	public Goal goalInstance;
	private byte level = 1; // Текущий уровень

	// Точка вхождения игры
	void Update () {
		// По нажатию пробела запускаем игру
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartGame();
		}
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

	// Переход на следующий уровень
	public void GoNextLevel() {
		level++;

		// Удаляем всё
		var cells = FindObjectsOfType(typeof(MazeCell)) as MazeCell[];
		foreach (var cell in cells)
			Destroy(cell.gameObject);
		Destroy(GameObject.Find("Goal"));
		Destroy(GameObject.Find("Circle"));

		// И пересоздаем
		mazeInstance.InstantiateMaze(level);
		CreateCircle();
		CreateGoal();

		// Выставляем камеру
		SetUpCam();
	}
}
