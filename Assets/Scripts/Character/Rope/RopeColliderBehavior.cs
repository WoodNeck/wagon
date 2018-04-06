using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeColliderBehavior : MonoBehaviour {
    public HeroActor hero {get; set;}
    SpriteRenderer spriteRenderer;
    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update(){
        spriteRenderer.sortingOrder = hero.spriteRenderer.sortingOrder - 1;
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!hero.isLocalPlayer) return;
        if (!collider.isTrigger) return;
        GameObject ropedObject = collider.transform.root.gameObject;
        Ropeable ropeable = ropedObject.GetComponent<Ropeable>();

        if (ropeable != null){
            if (ropeable.Equals(hero)) return;

            if (ropeable.CanRoped()){
                hero.ropeController.CmdAttachRope(ropedObject);
            }
        }
	}
}
