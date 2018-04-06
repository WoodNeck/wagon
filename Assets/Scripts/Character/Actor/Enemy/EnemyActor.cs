using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyActor : BaseActor, Damageable, Ropeable {
    public EnemyStateManager stateManager {get; protected set;}
    protected override void Awake(){
        base.Awake();
        hp = GetComponent<HealthContainer> ();
		hp.transform.parent = transform;
    }

    public override void Die(GameObject attackedObject){
        if (!isDead){
            isDead = true;
            body.velocity = Vector3.zero;
            moveInput = Vector2.zero;
            animator.SetTrigger ("Dead");
        }
    }

    public override void Damage(GameObject attackedObject, float amount){
        hp.Damage(attackedObject, amount);
        stateManager.EventDamaged();
    }

    protected override void FixedUpdate () {
        base.FixedUpdate ();
        if (!isDead){
            stateManager.FindNextState();
            stateManager.PerformStateAction();
        }
    }

    public override bool CanMove() {
        if      (isDead)                       return false;
        else if (state == ActorState.ROPED)    return false;
        else if (state == ActorState.ATTACK)   return false;
        else if (state == ActorState.DISABLED) return false;
        return true;
    }

    public override void EventGround(){
        // DO NOTHING
    }

    public virtual void AttachRope(RopeBehavior rope){
        rope.ropeHolder.spriteRenderer.size = new Vector2 (0.15f, 0.03f);
        body.velocity = Vector2.zero;
        moveInput = Vector2.zero;
        body.drag = 100f;
        animator.SetTrigger("Roped");
        stateManager.EventRoped();
    }

    public virtual void DetachRope(RopeBehavior rope){
        body.velocity = Vector2.zero;
        moveInput = Vector2.zero;
        body.drag = 0f;
        if (!isDead){
            animator.SetTrigger("Unroped");
        }
    }

    public virtual void ApplyRopeForce(Vector2 forceDir){
        body.AddForce(forceDir);
    }

    public virtual Vector2 AnchorPos(){
        return moveCollider.offset;
    }

    public virtual bool CanRoped(){
        if (isDead) return false;
        return true;
    }
}
