using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttribute : EnemyAttribute
{
    [HideInInspector]
    public bool firstInBurst = false;
    public override void Reset()
    {
        attackDamage = 10f;
        maxHp = 200f;
        currentHp = maxHp;
        isAlive = true;
        firstInBurst = false;
        firstInDead = false;
        enemyName = "Skeleton Warrior";
    }

    public float GetAttackDamage()
    {
        return (firstInBurst) ? attackDamage * 1.35f : attackDamage;  
    }
    

}
