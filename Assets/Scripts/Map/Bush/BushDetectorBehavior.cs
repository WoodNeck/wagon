using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BushDetectorBehavior : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D collider){
        if (!NetworkServer.active) return;
        if (collider.transform.root.tag == "Player" || collider.transform.root.tag == "Wagon"){
            BushBehavior bush = transform.parent.GetComponent<BushBehavior>();
            if (bush.enemyToSpawn != null){
                GameObject enemy = Instantiate(bush.enemyToSpawn);
                enemy.transform.position = transform.position;
                NetworkServer.Spawn(enemy);
                Destroy(this.gameObject);
            }
        }
    }
}
