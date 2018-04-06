using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudTileBehavior : MonoBehaviour {
	public Sprite[] tileSet;

	void Awake () {
		int tileSpriteIndex = UnityEngine.Random.Range (0, tileSet.Length);
		gameObject.GetComponent<SpriteRenderer> ().sprite = tileSet[tileSpriteIndex];
	}

}