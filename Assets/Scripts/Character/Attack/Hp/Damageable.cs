using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Damageable {
    void Damage(GameObject attackedObject, float amount);
    void Die(GameObject killer);
}