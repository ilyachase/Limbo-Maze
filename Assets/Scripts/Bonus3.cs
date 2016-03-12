using UnityEngine;
using System.Collections;

public class Bonus3 : MonoBehaviour {

	// Use this for initialization
	public void InstantiateBonus(byte depth) {
		int x = 0, y = 0;

		// Получаем координаты для позиционирования бонуса
		var goal = GameObject.Find("Goal").GetComponent<Goal>();
		goal.FindGoalCoordinates(ref x, ref y, depth);

		transform.position = new Vector2(x, y);
	}
}
