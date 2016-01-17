using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	// Точка вхождения игры
	void Update () {
		// По нажатию пробела запускаем игру
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartGame();
		}
	}

	void StartGame() {
		Debug.Log("Game started");
	}
}
