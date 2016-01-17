using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {

	// При инстанциации, лабиринт должен генирироваться
	void Start () {
		GenerateMaze();
	}
	
	// Генерация лабиринта
	void GenerateMaze () {
		Debug.Log("Maze generated");
	}
}
