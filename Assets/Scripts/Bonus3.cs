using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bonus3 : MonoBehaviour {

	// Use this for initialization
	public void InstantiateBonus(byte depth) {
		int x = 0, y = 0;

		// Получаем координаты для позиционирования бонуса
		var goal = GameObject.Find("Goal").GetComponent<Goal>();
		goal.FindGoalCoordinates(ref x, ref y, depth);

		transform.position = new Vector2(x, y);
	}

	// Возможность пройти через стенку, зажав шифт
	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "Circle") {
			var cir = GameObject.Find("Circle").GetComponent<Circle>();
			cir.AddBonus(3);
			Destroy(gameObject);
			GameObject.Find("Bonuses").GetComponent<Text>().color = Color.white;
			GameObject.Find("Bonus3_icon").GetComponent<Image>().enabled = true;
			if (GameObject.Find("Game Manager").GetComponent<GameManager>().level == 6)
				GameObject.Find("Bonus3_tip").GetComponent<Bonus3_tip>().Show();
		}
	}
}
