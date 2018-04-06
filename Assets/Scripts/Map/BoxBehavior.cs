using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour, Damageable {
    int damageCount = 0;
    Collider2D moveCollider;
    Collider2D hitCollider;
    bool isDead = false;


    void Awake(){
        moveCollider = transform.Find("MoveCollider").GetComponent<Collider2D>();
        hitCollider = transform.Find("HitCollider").GetComponent<Collider2D>();
    }

    void Update(){
        if (!isDead){
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sprite in sprites){
                sprite.sortingOrder = (int)(-100 * (transform.position.y + moveCollider.offset.y));
            }
        }
    }

    public void Die(GameObject attackedObject){
        isDead = true;
        StartCoroutine (BreakBox());
    }

    public void Damage(GameObject attackedObject, float amount){
        damageCount++;
        if (damageCount >= 1){
            if (!isDead){
                Die(attackedObject);
            }
        }
    }

	IEnumerator BreakBox() {
		SpriteRenderer[] boxFrags = transform.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer boxFrag in boxFrags){
			Rigidbody2D fragRigidbody = boxFrag.gameObject.AddComponent<Rigidbody2D> ();
			fragRigidbody.AddForce (new Vector2(Random.Range(-4, 4), Random.Range(2, 8)), ForceMode2D.Impulse);
			fragRigidbody.AddTorque (Random.Range(-20f, 20f), ForceMode2D.Impulse);
		}
        Destroy(moveCollider.gameObject);
        Destroy(hitCollider.gameObject);
		yield return new WaitForSeconds (2f);
		Destroy (this.gameObject);
	}
}
