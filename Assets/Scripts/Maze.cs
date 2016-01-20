using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
	public int width, height; // Ширина и высота лабиринта

	private byte[,] mazeMatrix; // Матрица лабиринта; 0 - непосещенная ячейка, 1 - стенка, 2 - посещенная ячейка

	// Структура клетки
	private struct cell {
		public int x, y;

		public cell(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	// При инстанциации, лабиринт должен генирироваться
	void Start () {
		GenerateMaze();
	}

	// Возвращает массив непосещенных соседей клетки
	List<cell> getNeighbours(cell c) {
		var res = new List<cell>();
		int x = c.x, y = c.y;

		// Проверяем четыре направления (вверх, вправо, вниз, влево)
		if ((y - 2 > 0) && (mazeMatrix[y - 2, x] == 0))
			res.Add(new cell(x, y-2));
		if ((x + 2 < width) && (mazeMatrix[y, x + 2] == 0))
			res.Add(new cell(x+2, y));
		if ((y + 2 < height) && (mazeMatrix[y + 2, x] == 0))
			res.Add(new cell(x, y+2));
		if ((x - 2 > 0) && (mazeMatrix[y, x - 2] == 0))
			res.Add(new cell(x-2, y));

		return res;
	}

	// Убирает стену между двумя ячейками (превращает в посещенную ячейку)
	void removeWall(cell f, cell t) {
		// Клетки либо на одной строке
		if (f.x == t.x)
			if (f.y > t.y)
				mazeMatrix[f.y - 1, f.x] = 2;
			else
				mazeMatrix[f.y + 1, f.x] = 2;

		// Либо в одном столбце
		else if (f.y == t.y)
			if (f.x > t.x)
				mazeMatrix[f.y, f.x - 1] = 2;
			else
				mazeMatrix[f.y, f.x + 1] = 2;
	}

	// Генерация лабиринта
	void GenerateMaze () {
		mazeMatrix = new byte[width, height];
		var goodCells = new Stack<cell>(); // Стек клеток с непосещенными соседями

		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				if ((i % 2 != 0 && j % 2 != 0) && // Если ячейка нечетная по x и y, 
				   (i < height - 1 && j < width - 1)) // И при этом находится в пределах стен лабиринта
					mazeMatrix[i,j] = 0; // То это пустая КЛЕТКА
				else mazeMatrix[i,j] = 1; // Иначе это СТЕНА
			}
		}

		// Непосредственно генерация лабиринта методом поиска в глубину
		int unvisitedCells = ((width-2) / 2 + 1) * ((height - 2) / 2 + 1); // Кол-во непосещенных ячеек
		var currentCell = new cell(1, 1); // Начальная ячейка
		mazeMatrix[currentCell.y, currentCell.x] = 2; // Начальная ячейка - посещенная
		unvisitedCells--;
		do {
			var neighbours = getNeighbours(currentCell); // Получаем соседей текущей точки
			if (neighbours.Count > 0) { // Если есть соседи
				goodCells.Push(currentCell);
				int rand = Random.Range(0, neighbours.Count);
				removeWall(currentCell, neighbours[rand]);
				currentCell = neighbours[rand];
				mazeMatrix[currentCell.y, currentCell.x] = 2;
				unvisitedCells--;
			}
			else if (goodCells.Count > 0) { // Если нет соседей, берем ячейку из стека
				currentCell = goodCells.Pop();
			}
		}
		while (unvisitedCells > 0);

		outp();
	}

	void outp() {
		string o = "";
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++)
				o += (mazeMatrix[i, j] == 1) ? "■" : " ";
			o += "\n";
		}
		Debug.Log(o);
	}
}
