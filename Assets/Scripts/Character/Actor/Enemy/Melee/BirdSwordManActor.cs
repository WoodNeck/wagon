using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSwordManActor : EnemyActor {
    protected override void Awake(){
        base.Awake();
        stateManager = new BirdSwordManStateManager(this);
        hp.SetMaxHealth(3);
    }

    public override void Die(GameObject attackedObject){
        base.Die(attackedObject);
        heighter.SetVelocity(1.5f);
        ForceMove((transform.position - attackedObject.transform.position).normalized);
    }

    public override void Damage(GameObject attackedObject, float amount){
        base.Damage(attackedObject, amount);
        if (!isDead){
            animator.SetTrigger("Damaged");
        }
    }

    public override void EventGround(){
        stateManager.EventGround();
        if (isDead){
            animator.SetTrigger("Dead2");
            ForceMove(Vector2.zero);
        }
    }

    void OnGUI(){
        Vector3 pos = Camera.main.WorldToScreenPoint (transform.position);
        float scale = Screen.dpi / 96.0f;
        GUIStyle boxStyle = new GUIStyle (GUI.skin.label);
        boxStyle.fontSize = (int) (20 * scale);

        Rect charPos = new Rect (pos.x + 22, Screen.height - pos.y - 20, 400, 100);
        GUI.Label (charPos, stateManager.currentAction.GetType().FullName, boxStyle);
    }
}
