using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType { TOGGLE, HOLD }

public interface Interactable {
    GameObject gameObject {get;}
    bool CanInteract ();
	InteractType Interact (HeroActor hero);
    void StopInteract (HeroActor hero);
}