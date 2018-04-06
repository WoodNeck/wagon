using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushLeafBehavior : MonoBehaviour {
	void Start () {
		StartCoroutine(Die());
	}

    IEnumerator Die(){
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
