using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {
	public Goal goalPrefab;
	public float speed = 0.3f;
	public Maze mazeInstance;

	bool have_b1 = false, have_b2 = false, have_b3 = false;
	Vector2 dest = Vector2.zero;
	float ghost_time = 0;
	bool blocked = false;

	// Помещаем персонажа в центр лабиринта
	void Start() {
		var maze = GameObject.Find("Maze").GetComponent<Maze>();
		transform.position = new Vector2(maze.width / 2, maze.height / 2);
		dest = transform.position;

		mazeInstance = GameObject.Find("Maze").GetComponent<Maze>();
	}

	void FixedUpdate() {
		if (blocked)
			return;

		if (ghost_time > 0) {
			ghost_time -= Time.deltaTime;
			if (ghost_time <= 0)
				speed = 0.3f;
		}

		// Move closer to Destination
		Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
		GetComponent<Rigidbody2D>().MovePosition(p);

		// Активация бонусов
		if (Input.GetKey(KeyCode.LeftShift)) {
			// Первый бонус
			if (have_b1) {
				ghost_time = 5f;
				speed = 0.5f;
				have_b1 = false;
			}

			// Второй бонус
			else if (have_b2) {
				var c = GameObject.Find("Counter").GetComponent<Counter>();
				c.ActiveteBonus2();
				have_b2 = false;
			}

			// Третий бонус
			// Если вместе с шифтом нажата стрелка, есть бонус
			else if (((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.LeftArrow))) && (have_b3)) {
				Vector2 new_dest = dest;

				// Если уперлись в стену, то пытаемся пройти через одну тонкую стенку
				if (Input.GetKey(KeyCode.UpArrow) && !valid(Vector2.up)) {
					var is_there_a_wall = GameObject.Find("Maze Cell " + (new_dest.y + 2) + ", " + new_dest.x);
					if ((is_there_a_wall == null) && (new_dest.y + 2 <= mazeInstance.height - 2))
						new_dest.y += 2;
				}
				else if (Input.GetKey(KeyCode.RightArrow) && !valid(Vector2.right)) {
					var is_there_a_wall = GameObject.Find("Maze Cell " + new_dest.y + ", " + (new_dest.x + 2));
					if ((is_there_a_wall == null) && (new_dest.x + 2 <= mazeInstance.width - 2))
						new_dest.x += 2;
				}
				else if (Input.GetKey(KeyCode.DownArrow) && !valid(-Vector2.up)) {
					var is_there_a_wall = GameObject.Find("Maze Cell " + (new_dest.y - 2) + ", " + new_dest.x);
					if ((is_there_a_wall == null) && (new_dest.y - 2 >= 1))
						new_dest.y -= 2;
				}
				else if (Input.GetKey(KeyCode.LeftArrow) && !valid(-Vector2.right)) {
					var is_there_a_wall = GameObject.Find("Maze Cell " + new_dest.y + ", " + (new_dest.x - 2));
					if ((is_there_a_wall == null) && (new_dest.x - 2 >= 1))
						new_dest.x -= 2;
				}

				if (new_dest != dest) {
					dest = new_dest;
					have_b3 = false;
				}
			}
		}

		// Check for Input if not moving
		if ((Vector2)transform.position == dest) {
			if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
				dest = (Vector2)transform.position + Vector2.up;
			if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
				dest = (Vector2)transform.position + Vector2.right;
			if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
				dest = (Vector2)transform.position - Vector2.up;
			if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
				dest = (Vector2)transform.position - Vector2.right;
		}
	}

	bool valid(Vector2 dir) {
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider.gameObject.tag != "Wall");
	}

	public void AddBonus(byte b) {
		if (b == 1)
			have_b1 = true;
		else if (b == 2)
			have_b2 = true;
		else
			have_b3 = true;
	}

	public void Block() {
		if (!blocked)
			blocked = true;
	}

	public void Unblock() {
		if (blocked)
			blocked = false;
	}

	public void Move(Vector2 new_dest) {
		dest = new_dest;
	}
}
