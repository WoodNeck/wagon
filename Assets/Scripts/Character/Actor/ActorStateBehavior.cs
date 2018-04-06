using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStateBehavior : StateMachineBehaviour {
    public ActorState stateToApply;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        BaseActor actor = animator.GetComponent<BaseActor>();
        actor.state = stateToApply;
    }
}
