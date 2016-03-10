using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour {
	public Goal goalPrefab;
    public float speed = 0.3f;
    Vector2 dest = Vector2.zero;

    // Помещаем персонажа в центр лабиринта
    void Start() {
		var maze = GameObject.Find("Maze").GetComponent<Maze>();
		transform.position = new Vector2(maze.width / 2, maze.height / 2);
        dest = transform.position;
    }

    void FixedUpdate() {
        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

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
		var hitname = hit.collider.gameObject.name;
		return (hitname == "Circle") || (hitname == "Goal");
    }
}
