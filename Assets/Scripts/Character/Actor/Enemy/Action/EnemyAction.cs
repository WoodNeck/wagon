using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType {PLAYER, WAGON, BOTH}

public interface EnemyAction {
    bool isOver{ get; }
    void Execute();
}
