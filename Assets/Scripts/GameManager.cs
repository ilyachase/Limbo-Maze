using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab; // reference на prefab лабиринта
    public Circle circlePrefab;  // reference на prefab персонажа
	public Goal goalPrefab;

    private Maze mazeInstance; // instance лабиринта

	// Точка вхождения игры
	void Update () {
		// По нажатию пробела запускаем игру
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartGame();
		}
	}

	// Запуск игры
	void StartGame() {
		// Если лабиринта ещё нет, инстанциируем его
		if (GameObject.Find("Maze") == null) {
            // Создаем лабиринт
			mazeInstance = Instantiate(mazePrefab) as Maze;
			mazeInstance.name = "Maze";

            // Создаем персонажа
            var circle = Instantiate(circlePrefab) as Circle;
            circle.name = "Circle";

			// Создаем цель
			var goal = Instantiate(goalPrefab) as Goal;
			goal.name = "Goal";

            // Выставляем камеру
            var cam = GameObject.Find("Main Camera").GetComponent<Camera>();
			cam.transform.position = new Vector3(mazeInstance.width * 0.49f, mazeInstance.height * 0.5f, -10);
			cam.orthographicSize = mazeInstance.width * 0.58f;
        }
    }
}
