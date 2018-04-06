using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Ropeable {
    void AttachRope(RopeBehavior rope);
    void DetachRope(RopeBehavior rope);
    void ApplyRopeForce(Vector2 forceDir);
    bool CanRoped();
    Vector2 AnchorPos();
}
