using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChecker : MonoBehaviour {
    public Environment current {get; set;}
    public Vector3 colliderOffset {get; set;}
    void Awake(){
        current = new NullEnvironment();
        colliderOffset = Vector3.zero;
    }

	void Update () {
        CheckEnvironment();
	}

    public void CheckEnvironment(){
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position + colliderOffset);
        bool envEntered = false;
        foreach(Collider2D collider in colliders){
            Environment newEnvironment = collider.GetComponent<Environment>();
            if (newEnvironment != null){
                if (current.GetType() != newEnvironment.GetType()){
                    newEnvironment.Enter(this.gameObject);
                    current.Exit(this.gameObject);
                    current = newEnvironment;
                } else {
                    newEnvironment.Stay(this.gameObject);
                }
                envEntered = true;
                break;
            } else {
                continue;
            }
        }

        if (!envEntered){
            current.Exit(this.gameObject);
            current = new NullEnvironment();
        }
    }

/*     public float CurrentHeight(){
        bool isFlying = ownerHeighter.height > 0;
        if (isFlying){
            Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position + colliderOffset);
            foreach(Collider2D collider in colliders){
                Environment newEnvironment = collider.GetComponent<Environment>();
                if (newEnvironment != null){
                    return newEnvironment.Height(transform.position);
                }
            }
        }
        return current.Height(transform.position);
    }

    public float Height(Vector3 position){
        Collider2D[] colliders = Physics2D.OverlapPointAll(position + colliderOffset);
        foreach(Collider2D collider in colliders){
            Environment newEnvironment = collider.GetComponent<Environment>();
            if (newEnvironment != null){
                return newEnvironment.Height(position);
            }
        }
        return new NullEnvironment().Height(position);
    } */
}
