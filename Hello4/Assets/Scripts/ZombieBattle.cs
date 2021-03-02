using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBattle : EnemyBattle
{

    ZombieAnimation zombieAnimation;
    AIController aIController;
    public override void Awake()
    {
        base.Awake();
        aIController = this.GetComponent<AIController>();
        zombieAnimation = this.GetComponent<ZombieAnimation>();
    }


    public override void TakeDamage(ChangeInfo info)
    {
        base.TakeDamage(info);

        zombieAnimation.ChangeAnimation(ZombieAnimation.State.Damaged);
        aIController.StateChange(AIController.FSMState.Damaged);

    }
}
