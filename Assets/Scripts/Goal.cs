using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal : MonoBehaviour {
	// Нахождение координат для постановки цели
	// Смысл - вставляем цель вплотную к внешней границе, но в любое свободное место из этой границы
	void FindGoalCoordinates(ref int x, ref int y) {
		Maze maze = GameObject.Find("Maze").GetComponent<Maze>();

		bool found = false;
		int side;

		do {
			side = Random.Range(1, 4); // Сначала выбираем сторону

			switch (side) {
				// Верхняя
				case 1:
					y = 1;
					x = Random.Range(1, maze.width - 2);
				break;

				// Правая
				case 2:
					y = Random.Range(1, maze.height - 2);
					x = maze.width - 2;
				break;

				// Нижняя
				case 3:
					y = maze.height - 2;
					x = Random.Range(1, maze.width - 2);
				break;

				// Левая
				case 4:
					y = Random.Range(1, maze.height - 2);
					x = 1;
				break;
			}

			if (maze.mazeMatrix[y, x] != 1)
				found = true;
		}
		while (!found);
	}

	// Use this for initialization
	void Start () {
		int x = 0, y = 0;

		FindGoalCoordinates(ref x, ref y);
		transform.position = new Vector2(x, y);
	}

	// Переходим на следующий уровень, если нас достиг персонаж
	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "Circle")
			GameObject.Find("Game Manager").GetComponent<GameManager>().GoNextLevel();
	}
}
