using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleTrapBehavior : MonoBehaviour {

	Animator _animator;

	void Awake(){
		GetComponent<SpriteRenderer> ().sortingOrder = (int) -(gameObject.transform.position.y * 10);
		_animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Actor")
			_animator.SetTrigger ("Activated");
	}
}
