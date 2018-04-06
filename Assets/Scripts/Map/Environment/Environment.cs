using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Environment {
    Vector2 ModifyMove(GameObject obj, Vector2 velocity);
    float Height(Vector3 position);

    void Enter(GameObject obj);
    void Stay(GameObject obj);
    void Exit(GameObject obj);
}
