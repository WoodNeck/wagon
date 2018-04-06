using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RopeController : NetworkBehaviour {
    public GameObject ropePrefab;
    HeroActor hero;
    [SyncEvent] event GameObjectDelegate EventRopeAttach;
    [SyncEvent] event VoidDelegate EventRopeDetach;

	void Awake () {
		hero = GetComponent<HeroActor>();
        EventRopeAttach = ApplyRopeAttach;
        EventRopeDetach = ApplyRopeDetach;
	}

    [Command] public void CmdAttachRope(GameObject ropedObject){
        EventRopeAttach(ropedObject);
    }

    void ApplyRopeAttach(GameObject ropedObject){
        if (hero.rope != null) return;

        hero.animator.SetTrigger("RopeSuccess");
        RopeBehavior rope = Instantiate (ropePrefab).GetComponent<RopeBehavior> ();
        rope.InitializeRope (base.gameObject, ropedObject);
        Ropeable roped = ropedObject.GetComponent<Ropeable>();
        roped.AttachRope(rope);
    }

    [Command] public void CmdDestroyRope () {
        EventRopeDetach ();
    }

    public void ApplyRopeDetach () {
        if (hero.rope != null) {
            hero.animator.SetTrigger("Unroped");
            hero.rope.targetCollider.transform.root.GetComponent<Ropeable> ().DetachRope (hero.rope);
            Destroy (hero.rope.gameObject);
            hero.rope = null;
        }
    }

    [Command] public virtual void CmdApplyRopeForce(Vector2 forceDir){
        RpcApplyRopeForce(forceDir);
    }

    [ClientRpc] public virtual void RpcApplyRopeForce(Vector2 forceDir){
        if (hero.rope != null) hero.rope.ApplyRopeForce(forceDir);
    }
}
