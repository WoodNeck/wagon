using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullEnvironment : Environment {
    public Vector2 ModifyMove(GameObject obj, Vector2 velocity){
        return velocity;
    }
    public float Height(Vector3 position){
        return 0;
    }

    public void Enter(GameObject obj){}
    public void Stay(GameObject obj){}
    public void Exit(GameObject obj){}
}
