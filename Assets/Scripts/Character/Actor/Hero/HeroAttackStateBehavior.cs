using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackStateBehavior : StateMachineBehaviour {
    HeroActor hero;
    float frame;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    hero = animator.GetComponent<HeroActor> ();
        hero.body.velocity = Vector2.zero;
        hero.attackCollider.GetComponent<HeroAttackColliderBehavior>().SetAttackCollider(false);
        frame = 0;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemyObject in enemies){
            if ((enemyObject.transform.position - hero.transform.position).sqrMagnitude <= Mathf.Pow(0.44f, 2)){
                EnemyActor enemy = enemyObject.GetComponent<EnemyActor>();
                enemy.stateManager.EventAttack(hero.gameObject);
            }
        }
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    hero = animator.GetComponent<HeroActor> ();
        hero.body.velocity = Vector2.zero;
        hero.attackCollider.GetComponent<HeroAttackColliderBehavior>().SetAttackCollider(true);
        frame = 0;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    Direction8 heroDir = hero.direction.dir8;
        Vector2 forceDir = Direction.ToVector(heroDir).normalized;
        frame++;
        
        float easingMultiplier = AttackMoveEasing(frame / 30);
        hero.ForceMove(forceDir * easingMultiplier * 2f);
	}

    float AttackMoveEasing(float t){
        var ts = (t)*t;
	    var tc = ts*t;
        return 1 - (0.55f*tc*ts + -3.05f*ts*ts + 6f*tc + -6f*ts + 3.5f*t);
    }
}
