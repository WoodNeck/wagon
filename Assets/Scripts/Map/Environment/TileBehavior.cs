using UnityEngine;
using System.Collections;

public class TileBehavior : MonoBehaviour {
	public Sprite[] tileSet;

	void Awake () {
		int tileSpriteIndex = UnityEngine.Random.Range (0, tileSet.Length);
		gameObject.GetComponent<SpriteRenderer> ().sprite = tileSet[tileSpriteIndex];
	}

}
