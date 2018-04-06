using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackColliderBehavior : MonoBehaviour {
    public float damageVal = 0.5f;
    HeroActor hero;
    SpriteRenderer colliderRenderer;
    Vector3 defaultOffset;

    void Awake(){
        hero = transform.root.GetComponent<HeroActor>();
        colliderRenderer = GetComponent<SpriteRenderer>();
        defaultOffset = transform.localPosition;
    }

    void Update(){
        if (hero.direction.dir4 == Direction4.WEST) {
            transform.localScale = new Vector3 (-1, 1, 1);
        } else {
            transform.localScale = new Vector3 (1, 1, 1);
        }
        colliderRenderer.sortingOrder = hero.spriteRenderer.sortingOrder;
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!collider.isTrigger) return;
        if (collider.tag != "HitCollider") return;
        if (collider.transform.parent == null) return;

        GameObject attackedObject = collider.transform.parent.gameObject;
        if (attackedObject.tag == "Player" || attackedObject.tag == "Wagon") return;
        Damageable damageable = attackedObject.GetComponent<Damageable>();

        if (damageable != null){
            hero.commander.CmdDamageObject(attackedObject, damageVal);
        }
	}

    public void SetAttackCollider(bool isReset){
        Vector2 dir;
        if (isReset){
            dir = Vector2.zero;
        }
        else {
            dir = Direction.ToVector(hero.direction.dir8);
        }
        if (dir.x < 0) dir = new Vector2(-dir.x, dir.y);
        transform.localPosition = defaultOffset + (Vector3) dir * 0.15f;

        Quaternion degree = Quaternion.identity;
        if      (hero.direction.dir8 == Direction8.EAST)      degree = Quaternion.Euler(0, 0, 0);
        else if (hero.direction.dir8 == Direction8.NORTH)     degree = Quaternion.Euler(0, 0, 90);
        else if (hero.direction.dir8 == Direction8.SOUTH)     degree = Quaternion.Euler(0, 0, 270);
        else if (hero.direction.dir8 == Direction8.WEST)      degree = Quaternion.Euler(0, 0, 180);
        else if (hero.direction.dir8 == Direction8.NORTHEAST) degree = Quaternion.Euler(0, 0, 45);
        else if (hero.direction.dir8 == Direction8.NORTHWEST) degree = Quaternion.Euler(0, 0, 225);
        else if (hero.direction.dir8 == Direction8.SOUTHEAST) degree = Quaternion.Euler(0, 0, 315);
        else if (hero.direction.dir8 == Direction8.SOUTHWEST) degree = Quaternion.Euler(0, 0, 135);

        transform.localRotation = degree;
    }
}
