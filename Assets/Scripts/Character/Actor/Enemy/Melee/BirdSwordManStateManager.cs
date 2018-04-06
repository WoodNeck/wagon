using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSwordManStateManager : EnemyStateManager {
    public BirdSwordManStateManager(EnemyActor owner) : base(owner) {
        currentAction = new WaitAction();
    }

    public override void FindNextState(){
        if (currentAction is WaitAction){
            currentAction = new WalkAction(owner, TargetType.PLAYER);
        } else if (currentAction is WalkAction){
            GameObject nearestPlayer = FindNearestPlayer();
            if ((nearestPlayer.transform.position - owner.transform.position).sqrMagnitude <= Mathf.Pow(0.22f, 2f)){
                currentAction = new AttackPrepareAction(owner, nearestPlayer, 0.4f);
            }
        } else if (currentAction is AttackPrepareAction){
            if (currentAction.isOver){
                AttackPrepareAction prevAction = currentAction as AttackPrepareAction;
                currentAction = new MeleeAttackAction(owner, prevAction.targetPosition, 1f, 2f);
            }
        } else if (currentAction is MeleeAttackAction){
            if (currentAction.isOver){
                owner.animator.SetTrigger("Idle");
                currentAction = new WaitAction();
            }
        }
    }

    public override void EventGround(){
        currentAction = new WaitAction();
    }

    public override void EventAttack(GameObject attackedObject){
        
    }

    public override void EventDamaged(){
        currentAction = new WaitAction();
    }

    public override void EventRoped(){
        currentAction = new WaitAction();
    }
}
