using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAction : EnemyAction {
    public bool isOver {get; private set;}
    EnemyActor owner;
    TargetType type;
    public WalkAction(EnemyActor owner, TargetType type){
        this.owner = owner;
        this.type = type;
    }

    public void Execute(){
        if (owner.CanMove()){
            GameObject nearest;
            if (type == TargetType.PLAYER) {
                nearest = owner.stateManager.FindNearestPlayer();
            } else if (type == TargetType.WAGON) {
                nearest = owner.stateManager.FindWagon();
            } else {
                nearest = owner.stateManager.FindNearestObject();
            }
            Vector2 distance = (Vector2) (nearest.transform.position - owner.transform.position);
            if (distance.sqrMagnitude >= Mathf.Pow(0.22f, 2)){
                Vector2 direction = distance.normalized;
                owner.Move(direction * 0.3f);
            } else {
                owner.Move(Vector2.zero);
            }
        } else {
            owner.Move(Vector2.zero);
        }
    }
}
