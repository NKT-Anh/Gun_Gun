using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Stats/EnemyStats")]
public class EnemyStats : ActorStats
{
    [Header("Xp_Bonus: ")]
    public float minXPBonus;
    public float maxXPBonus;
    [Header("LevelUp")]
    public float hpUp;
    public float damageUp;

}
