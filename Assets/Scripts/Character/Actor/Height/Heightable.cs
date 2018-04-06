using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Heightable {
    GameObject gameObject {get;}
    HeightBehavior heighter {get;}
    void EventGround();
}
