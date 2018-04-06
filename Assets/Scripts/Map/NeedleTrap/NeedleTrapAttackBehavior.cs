using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleTrapAttackBehavior : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage ("Damage", 1f);
		}
	}
}
