using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushBehavior : MonoBehaviour {
    public GameObject enemyToSpawn;
    public GameObject bushLeafPrefab;
    public Sprite[] bushLeaf;
    Animator animator;  
    void Awake(){
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collider){
        Activate(collider);
    }

    void OnTriggerExit2D(Collider2D collider){
        Activate(collider);
    }

    void Activate(Collider2D collider){
        if (collider.isTrigger) return;
        Rigidbody2D colliderBody = collider.transform.root.GetComponent<Rigidbody2D>();
        if (colliderBody != null && colliderBody.velocity.sqrMagnitude > 0){
            animator.SetTriggerOneFrame("Activate");
        }
    }
}
