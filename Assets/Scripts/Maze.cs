using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
	public int width, height; // Ширина и высота лабиринта
	public byte[,] mazeMatrix; // Матрица лабиринта; 0 - непосещенная ячейка, 1 - стенка, 2 - посещенная ячейка
	public MazeCell cellPrefab;

	MazeCell[,] cells;
	List<byte[,]> Levels = new List<byte[,]>(); // Массив матриц уровней
	byte base_sum = 7;
	Circle circleInstance;

	// Структура клетки
	private struct cell {
		public int x, y;

		public cell(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}

	// Создаем физическое отображение лабиринта
	public void InstantiateMaze(byte level) {
		mazeMatrix = Levels[level - 1];
		width = height = base_sum + level * 4;

		cells = new MazeCell[width, height];

		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				if (mazeMatrix[y, x] == 1)
					CreateCell(x, y);
	}

	private void CreateCell(int x, int y) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
		cells[x, y] = newCell;
		newCell.name = "Maze Cell " + y + ", " + x;
		newCell.transform.parent = transform;
		newCell.transform.localPosition = new Vector2(x, y);
	}

	// Возвращает массив непосещенных соседей клетки
	List<cell> getNeighbours(cell c, ref byte[,] mazeMatrix) {
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
	void removeWall(cell f, cell t, ref byte[,] mazeMatrix) {
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
	public void GenerateLevels (bool regenerate_current = false, byte level = 0, bool dynamically = false) {
		int c_x = 0, c_y = 0;
		if (dynamically) {
			circleInstance = GameObject.Find("Circle").GetComponent<Circle>();
			c_x = (int)circleInstance.transform.position.x;
			c_y = (int)circleInstance.transform.position.y;
		}

		// Генерируем 8 уровней
		for (int lev = 1; lev <= 8; lev++) {
			if (regenerate_current)
				lev = level;

			width = height = base_sum + lev * 4;

			var mazeTemp = new byte[width, height];
			var goodCells = new Stack<cell>(); // Стек клеток с непосещенными соседями

			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					if ((i % 2 != 0 && j % 2 != 0) && // Если ячейка нечетная по x и y, 
					   (i < height - 1 && j < width - 1)) // И при этом находится в пределах стен лабиринта
						mazeTemp[i, j] = 0; // То это пустая КЛЕТКА
					else mazeTemp[i, j] = 1; // Иначе это СТЕНА
				}
			}

			// Непосредственно генерация лабиринта методом поиска в глубину
			int unvisitedCells = ((width - 2) / 2 + 1) * ((height - 2) / 2 + 1); // Кол-во непосещенных ячеек
			var currentCell = new cell(width / 2, height / 2); // Начальная ячейка
			bool first = true;
			int rand;

			mazeTemp[currentCell.y, currentCell.x] = 2; // Начальная ячейка - посещенная
			unvisitedCells--;
			do {
				var neighbours = getNeighbours(currentCell, ref mazeTemp); // Получаем соседей текущей точки
				if (neighbours.Count > 0) { // Если есть соседи
					goodCells.Push(currentCell);

					// Первую ячейку выбираем верхней от центра
					if (first) {
						rand = 0;
						first = false;
					} else
						rand = Random.Range(0, neighbours.Count);

					removeWall(currentCell, neighbours[rand], ref mazeTemp);
					currentCell = neighbours[rand];
					mazeTemp[currentCell.y, currentCell.x] = 2;
					unvisitedCells--;
				} else if (goodCells.Count > 0) { // Если нет соседей, берем ячейку из стека
					currentCell = goodCells.Pop();
				}
			}
			while (unvisitedCells > 0);

			if (dynamically) {
				if (mazeTemp[c_y, c_x] != 1) {
					Levels[level - 1] = mazeTemp;
					circleInstance.Move(new Vector2(c_x, c_y));
					return;
				}
			}
			else if (regenerate_current) {
				Levels[level - 1] = mazeTemp;
				return;
			}
			else
				Levels.Add(mazeTemp);
		}
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
