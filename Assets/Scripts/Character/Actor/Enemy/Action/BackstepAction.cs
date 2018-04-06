using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackstepAction : EnemyAction {
    public bool isOver{get; private set;}
    EnemyActor owner;
    Vector2 direction;
    float speed;

    public BackstepAction(EnemyActor owner, Vector2 direction, float speed) {
        this.owner = owner;
        this.direction = direction;
        this.speed = speed;
        isOver = false;

        if (owner.heighter.velocity == 0){
            owner.heighter.SetVelocity(speed);
        }
    }

    public void Execute() {
        owner.ForceMove(direction * speed);
    }
}
