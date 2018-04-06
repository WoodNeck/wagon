using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHolderBehavior : MonoBehaviour {
    public SpriteRenderer spriteRenderer {get; private set;}
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	void Update () {
        if (transform.parent != null){
            spriteRenderer.sortingOrder = transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
	}
}
