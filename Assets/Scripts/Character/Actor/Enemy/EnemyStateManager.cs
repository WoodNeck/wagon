using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateManager {
    public EnemyAction currentAction {get; protected set;}
    protected EnemyActor owner;

    public EnemyStateManager(EnemyActor owner){
        this.owner = owner;
    }
    public abstract void FindNextState();
    public void PerformStateAction(){
        currentAction.Execute();
    }

    public virtual void EventGround(){
        // DO NOTHING
    }

    public virtual void EventAttack(GameObject attackedObject){
        // DO NOTHING
    }

    public virtual void EventDamaged(){
        // DO NOTHING
    }

    public virtual void EventRoped(){
        // DO NOTHING
    }

    public GameObject FindNearestPlayer(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = null;
        foreach(GameObject player in players){
            HeroActor hero = player.GetComponent<HeroActor>();
            if (hero.isDead) continue;
            if (nearest == null){
                nearest = player;
            } else {
                float prevDiff = (nearest.transform.position - owner.transform.position).sqrMagnitude;
                float nextDiff = (player.transform.position - owner.transform.position).sqrMagnitude;
                if (nextDiff < prevDiff)
                    nearest = player;
            }
        }
        return nearest;
    }
    
    public GameObject FindWagon(){
        return GameObject.FindGameObjectWithTag("Wagon");
    }

    public GameObject FindNearestObject(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject nearest = GameObject.FindGameObjectWithTag("Wagon");
        foreach(GameObject player in players){
            HeroActor hero = player.GetComponent<HeroActor>();
            if (hero.isDead) continue;
            float prevDiff = (nearest.transform.position - owner.transform.position).sqrMagnitude;
            float nextDiff = (player.transform.position - owner.transform.position).sqrMagnitude;
            if (nextDiff < prevDiff)
                nearest = player;
        }
        return nearest;
    }
}