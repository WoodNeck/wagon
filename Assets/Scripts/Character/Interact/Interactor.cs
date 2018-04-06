using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Interactor : NetworkBehaviour {
    public InteractableDetector interactableDetector {get; private set;}
    public bool isInteracting {get; private set;}
    Interactable interactingObject = null;
    InteractType interactingType;

    HeroActor hero = null;

    void Awake() {
        interactableDetector = transform.Find("InteractableDetector").GetComponent<InteractableDetector> ();
        hero = GetComponent<HeroActor>();
        isInteracting = false;
    }

    public void OnInteractKeyDown(){
        if (!isInteracting) {
            if (!interactableDetector.CanInteract()) return;

            Interactable interactable = interactableDetector.FindNearestObject();
            if (interactable != null && interactable.CanInteract()){
                CmdStartInteract(interactable.gameObject);
            }
        } else {
            if (interactingType == InteractType.TOGGLE){
                CmdStopInteract();
            }
        }
    }

    public void OnInteractKeyUp(){
        if (isInteracting && interactingType == InteractType.HOLD){
            CmdStopInteract();
        }
    }

    [Command] void CmdStartInteract(GameObject interactableObj){
        RpcStartInteract(interactableObj);
    }

    [Command] void CmdStopInteract(){
        RpcStopInteract();
    }

    [ClientRpc] void RpcStartInteract(GameObject interactableObj){
        Interactable interactable = interactableObj.GetComponent<Interactable>();
        if (interactable.CanInteract()){
            interactingType = interactable.Interact(hero);
            interactingObject = interactable;
            isInteracting = true;
        }
    }

    [ClientRpc] void RpcStopInteract(){
        interactingObject.StopInteract(hero);
        interactingObject = null;
        isInteracting = false;
    }

    public void FinishInteract(){
        interactingObject = null;
        isInteracting = false;
    }
}
