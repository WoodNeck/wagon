using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : HeroSkill {
    public void Execute(HeroActor hero){
        hero.hp.CmdHeal(1f);
    }
}
