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
		}
	}
}
