using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPrepareAction : EnemyAction {
    public GameObject target {get; private set;}
    public Vector3 targetPosition {get; private set;}
    public bool isOver{get; private set;}
    EnemyActor owner;
    float time;
    float waitTime;
    
    public AttackPrepareAction(EnemyActor owner, GameObject target, float waitTime) {
        this.owner = owner;
        this.targetPosition = target.transform.position;
        this.target = target;
        time = 0f;
        this.waitTime = waitTime;
        owner.animator.SetTrigger("Attack");
        isOver = false;
    }

    public void Execute() {
        owner.Move(Vector2.zero);

        time += Time.deltaTime;
        if (time >= waitTime) isOver = true;
    }
}
