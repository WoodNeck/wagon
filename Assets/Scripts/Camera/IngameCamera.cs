using UnityEngine;
using System.Collections;

public class IngameCamera : MonoBehaviour {
    WagonActor wagon = null;
	
	// Update is called once per frame
	void Update () {
        if (wagon == null){
            GameObject wagonObject = GameObject.FindGameObjectWithTag("Wagon");
            if (wagonObject != null){
                wagon = wagonObject.GetComponent<WagonActor>();
            }
        } else {
		    Vector3 newPos = new Vector3 (wagon.transform.position.x, wagon.transform.position.y, transform.position.z);
		    transform.position = newPos;
        }
	}
}
