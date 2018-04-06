using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightBehavior : MonoBehaviour{
    public float velocity { get; private set; }

    private float height = 0f;
    private Heightable owner;
    float gravity = 0.05f;
    List<GameObject> heightHoldingChilds = new List<GameObject>();

    void Awake(){
        owner = transform.root.GetComponent<Heightable>();
    }

    public void AddHeightHoldingChild(GameObject child){
        heightHoldingChilds.Add(child);
    }

    void FixedUpdate() {
        if (height > 0 || velocity != 0){
            velocity -= gravity;
            ModifyHeight();
        }
    }

    public void SetVelocity (float velocity){
        this.velocity = velocity;
    }

    void ModifyHeight() {
        bool willGround = false;
        if (height + velocity < 0f) {
            velocity = -height;
            willGround = true;
        }
        height += velocity;

        owner.gameObject.transform.Translate(new Vector3(0f, (velocity) / 100f));
        foreach(GameObject child in heightHoldingChilds){
            child.transform.Translate(new Vector3(0f, - velocity / 100f));
        }

        if (willGround){
            owner.EventGround();
            velocity = 0f;
            height = 0f;
        }
    }
}