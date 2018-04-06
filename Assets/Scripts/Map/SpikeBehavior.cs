using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour {
	void Awake(){
		GetComponent<SpriteRenderer> ().sortingOrder = (int)(-100 * (transform.position.y + GetComponent<Collider2D>().offset.y));
	}

	void OnCollisionEnter2D(Collision2D coll){
		Damageable damageable = coll.transform.GetComponent<Damageable>();
        if (damageable != null)
            damageable.Damage(this.gameObject, 0.5f);
	}
}
