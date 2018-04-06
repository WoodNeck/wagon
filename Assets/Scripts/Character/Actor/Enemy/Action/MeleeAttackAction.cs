using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : EnemyAction {
    public bool isOver {get; private set;}
    EnemyActor owner;
    float speed;
    float time;
    float targetTime;
    Vector2 attackDir;
    public MeleeAttackAction(EnemyActor owner, Vector3 targetPosition, float speed, float targetTime){
        owner.animator.SetTrigger("Attack");
        this.owner = owner;
        this.speed = speed;
        time = 0f;
        this.targetTime = targetTime;
        isOver = false;
        attackDir = (Vector2) (targetPosition - owner.transform.position).normalized;
    }

    public void Execute(){
        owner.ForceMove(attackDir * AttackMoveEasing(time / targetTime) * speed);
        time += Time.deltaTime;
        if (time >= targetTime) isOver = true;
    }

    float AttackMoveEasing(float t){
        var ts = (t)*t;
	    var tc = ts*t;
        return 1 - (0.55f*tc*ts + -3.05f*ts*ts + 6f*tc + -6f*ts + 3.5f*t);
    }
}
