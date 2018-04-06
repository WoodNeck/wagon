using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackColliderBehavior : MonoBehaviour {
    public float damageVal = 0.5f;
    EnemyActor owner;

    void Awake(){
        owner = transform.root.GetComponent<EnemyActor>();
    }

    void OnTriggerEnter2D (Collider2D collider) {
        if (!collider.isTrigger) return;
        if (collider.tag != "HitCollider") return;
        
        GameObject attackedObject = collider.transform.root.gameObject;
        if (attackedObject.tag == "Enemy") return;

        Damageable damageable = attackedObject.GetComponent<Damageable>();

        if (damageable != null){
            damageable.Damage(owner.gameObject, damageVal);
        }
	}
}