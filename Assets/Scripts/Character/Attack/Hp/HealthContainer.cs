using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthContainer : NetworkBehaviour {
    protected BaseActor actor;
	protected float hp;
	protected float maxHp;

    protected virtual void Awake(){
        actor = GetComponent<BaseActor>();
    }

	public virtual void SetMaxHealth(int value) {
		maxHp = (float) value;
		hp = maxHp;
    }

    [Command] public void CmdDamage(GameObject attackedObject, float amount){
        RpcDamage(attackedObject, amount);
    }

	[ClientRpc] public void RpcDamage(GameObject attackedObject, float amount){
        Damage(attackedObject, amount);
	}
    
    public virtual void Damage(GameObject attackedObject, float amount){
		// Make minimum unit of hp to 0.5
		amount = Mathf.FloorToInt (amount * 2) / 2f;
        hp = Mathf.Max(hp - amount, 0f);
        
        bool willDead = (hp <= 0);
		if (willDead) {
            if (!actor.isDead){
                actor.GetComponent<Damageable>().Die(attackedObject);
            }
        } else {
            actor.EventDamaged();
        }
    }

    [Command] public void CmdHeal(float amount){
        RpcHeal(amount);
    }

	[ClientRpc] public void RpcHeal(float amount){
        Heal(amount);
	}

    public virtual void Heal(float amount){
        if (!actor.isDead){
            // Make minimum unit of hp to 0.5
            amount = Mathf.FloorToInt (amount * 2) / 2f;
            hp = Mathf.Min(hp + amount, maxHp);
        }
    }
}
