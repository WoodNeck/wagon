using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehavior : MonoBehaviour {
    SpriteRenderer ownerRenderer;
    SpriteRenderer spriteRenderer;
    void Awake(){
        ownerRenderer = transform.root.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        spriteRenderer.sortingOrder = ownerRenderer.sortingOrder - 1;
    }
}
