using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public Maze mazePrefab; // reference на prefab лабиринта

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
			mazeInstance = Instantiate(mazePrefab) as Maze;
			mazeInstance.name = "Maze";
			
			// Выставляем камеру
			var cam = GameObject.Find("Main Camera").GetComponent<Camera>();
			cam.transform.position = new Vector3(mazeInstance.width * 0.49f, mazeInstance.height * 0.5f, -10);
			cam.orthographicSize = mazeInstance.width * 0.58f;
		}
	}
}
