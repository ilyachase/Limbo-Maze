using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Goal : MonoBehaviour {
	// Нахождение координат для постановки цели
	// Смысл - вставляем цель вплотную к внешней границе, но в любое свободное место из этой границы
	public void FindGoalCoordinates(ref int x, ref int y, byte depth = 0) {
		Maze maze = GameObject.Find("Maze").GetComponent<Maze>();

		bool found = false;
		int side;

		do {
			side = Random.Range(1, 4); // Сначала выбираем сторону

			switch (side) {
				// Верхняя
				case 1:
					y = 1 + depth;
					x = Random.Range(1 + depth, maze.width - 2 - depth);
				break;

				// Правая
				case 2:
					y = Random.Range(1 + depth, maze.height - 2 - depth);
					x = maze.width - 1 - depth;
				break;

				// Нижняя
				case 3:
					y = maze.height - 1 - depth;
					x = Random.Range(1 + depth, maze.width - 2 - depth);
				break;

				// Левая
				case 4:
					y = Random.Range(1 + depth, maze.height - 2 - depth);
					x = 1 + depth;
				break;
			}

			if (maze.mazeMatrix[y, x] != 1)
				found = true;
		}
		while (!found);
	}

	bool need_next_level = false;
	GameManager gmRef;
	Fader fRef;

	void Awake() {
		gmRef = GameObject.Find("Game Manager").GetComponent<GameManager>();
		fRef = GameObject.Find("Fader").GetComponent<Fader>();
	}

	// Use this for initialization
	void Start () {
		int x = 0, y = 0;

		FindGoalCoordinates(ref x, ref y);
		transform.position = new Vector2(x, y);
	}

	void Update() {
		if (need_next_level) {
			fRef.FadeOut();
			GetComponent<Renderer>().enabled = false;
			if (fRef.is_fading_out)
				return;

			gmRef.GoNextLevel();
		}
	}

	// Переходим на следующий уровень, если нас достиг персонаж
	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "Circle")
			need_next_level = true;
	}
}
