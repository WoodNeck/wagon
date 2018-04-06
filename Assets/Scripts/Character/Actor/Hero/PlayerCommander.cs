using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerCommander : NetworkBehaviour {
    public HeroActor hero {get; private set;}
	void Awake(){
        hero = GetComponent<HeroActor>();
    }

    [Command] public void CmdDamageObject(GameObject obj, float amount){
        RpcDamageObject(obj, amount);
    }

    [ClientRpc] public void RpcDamageObject(GameObject obj, float amount){
        Damageable objToDamage = obj.GetComponent<Damageable>();
        if (objToDamage == null) return;
        objToDamage.Damage(hero.gameObject, amount);
    }

    [Command] public void CmdKillObject(GameObject obj){
        RpcKillObject(obj);
    }

    [ClientRpc] public void RpcKillObject(GameObject obj){
        Damageable objToDamage = obj.GetComponent<Damageable>();
        if (objToDamage == null) return;
        objToDamage.Die(hero.gameObject);
    }
}
