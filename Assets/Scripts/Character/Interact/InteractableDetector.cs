using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDetector : MonoBehaviour {
    HeroActor hero;
    SpriteRenderer buttonRenderer;
    HashSet<Interactable> interactableObjects;

    void Awake(){
        hero = transform.parent.GetComponent<HeroActor>();
        buttonRenderer = transform.Find("ButtonGUI").GetComponent<SpriteRenderer>();
        interactableObjects = new HashSet<Interactable>();
    }

    void Start () {
        if (!hero.isLocalPlayer) {
            Destroy(this.gameObject);
        }
    }

    void Update(){
        if (CanInteract() && interactableObjects.Count > 0){
            Interactable nearest = FindNearestObject();
            if (nearest != null) {
                if (!nearest.CanInteract()) {
                    interactableObjects.Remove(nearest);
                    buttonRenderer.enabled = false;
                } else {
                    buttonRenderer.enabled = true;
                    buttonRenderer.gameObject.transform.position = nearest.gameObject.transform.position + new Vector3(0f, 0f, 0f);
                }
            }
        } else {
            buttonRenderer.enabled = false;
        }
    }

    void OnTriggerStay2D (Collider2D collider) {
        if (collider.isTrigger) return;
        if (CanInteract()){
            Interactable liftable = collider.transform.root.GetComponent<Interactable>();
            if (liftable == null) liftable = collider.transform.GetComponent<Interactable>();
            if (liftable != null) interactableObjects.Add(liftable);
        }
    }

    void OnTriggerExit2D (Collider2D collider) {
        if (CanInteract()){
            Interactable liftable = collider.transform.root.GetComponent<Interactable>();
            if (liftable == null) liftable = collider.transform.GetComponent<Interactable>();
            if (liftable != null) interactableObjects.Remove(liftable);
        }
    }

    public bool CanInteract(){
        if (!hero.CanMove())                 return false;
        if (hero.state == ActorState.ROPING)  return false;

        return true;
    }

    public Interactable FindNearestObject(){
        if (interactableObjects.Count <= 0) return null;

        Interactable nearest = null;
        foreach(Interactable nextLiftable in interactableObjects){
            if (nearest != null){
                Vector3 diffFormer = nearest.gameObject.transform.position - transform.position;
                Vector3 diffLatter = nextLiftable.gameObject.transform.position - transform.position;
                if (diffLatter.sqrMagnitude - diffFormer.sqrMagnitude < 0){
                    nearest = nextLiftable;
                }
            } else {
                nearest = nextLiftable;
            }
        }
        
        float sqrDistance = (hero.transform.position - nearest.gameObject.transform.position).sqrMagnitude;
        if (sqrDistance > Mathf.Pow(2 * GetComponent<CircleCollider2D>().radius, 2f)){
            interactableObjects.Remove(nearest);
            return FindNearestObject();
        } else {
            return nearest;
        }
    }

    public void Clear(){
        interactableObjects.Clear();
    }
}