using UnityEngine;
using System.Collections;

public class Maze : MonoBehaviour {
	public int width, height; // Ширина и высота лабиринта

	private int[,] mazeMatrix; // Матрица лабиринта, 

	// При инстанциации, лабиринт должен генирироваться
	void Start () {
		GenerateMaze();
	}
	
	// Генерация лабиринта
	void GenerateMaze () {
		mazeMatrix = new int[width, height];

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if ((i % 2 != 0 && j % 2 != 0) && // Если ячейка нечетная по x и y, 
				   (i < height - 1 && j < width - 1)) // И при этом находится в пределах стен лабиринта
					mazeMatrix[i,j] = 0; // То это пустая КЛЕТКА
				else mazeMatrix[i,j] = 1; // Иначе это СТЕНА
			}
		}outp();
	}

	void outp() {
		string o = "";
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++)
				o += mazeMatrix[i, j] + " ";
			o += "\n";
		}
		Debug.Log(o);
	}
}
