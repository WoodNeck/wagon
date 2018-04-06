using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAnimBehavior : StateMachineBehaviour {
    public GameObject ropeColliderPrefab;
    HeroActor hero;
    GameObject rope = null;
    Vector2 ropeDefaultPos = new Vector2(0, -0.08f);
    Vector2 ropeDefaultSize;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    hero = animator.GetComponent<HeroActor> ();
        hero.body.velocity = Vector2.zero;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        hero.ForceMove(Vector2.zero);
        if (stateInfo.normalizedTime >= 3 / 8){
            if (rope == null)
                rope = MakeRope();
            float multiplier = CalcMultiplier(stateInfo.normalizedTime);
            rope.GetComponent<SpriteRenderer>().size = new Vector2(ropeDefaultSize.x * multiplier, ropeDefaultSize.y);
            rope.GetComponent<BoxCollider2D>().size = new Vector2(ropeDefaultSize.x * multiplier, ropeDefaultSize.y);
            float increaseAmount = ropeDefaultSize.x * 0.5f * (multiplier - 1);
            Vector2 direction = Direction.ToVector(hero.direction.dir8).normalized;
            rope.transform.position = hero.transform.position + (Vector3) (ropeDefaultPos + direction * increaseAmount);
        }
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (rope != null){
            Destroy(rope);
        }
    }

    GameObject MakeRope(){
        rope = Instantiate(ropeColliderPrefab);
        rope.GetComponent<RopeColliderBehavior>().hero = hero;
        rope.transform.position = hero.transform.position + (Vector3) ropeDefaultPos;
        rope.transform.localRotation = Direction.ToDegree(hero.direction.dir8);
        ropeDefaultSize = rope.GetComponent<SpriteRenderer>().size;
        return rope;
    }

    float CalcMultiplier(float normalTime){
        Vector2 min = new Vector2(ropeDefaultSize.x, 0);
        Vector2 max = new Vector2(0.66f, 0);
        Vector2 result = Vector2.Lerp(min, max, normalTime);
        return result.x / ropeDefaultSize.x;
    }
}
