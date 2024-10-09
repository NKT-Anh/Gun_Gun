using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorStats : Stats
{
    public string playerName;
    public float hp;
    public float moveSpeed;
    public float damage;
    public float knockBackForce;
    public float knockBackTime;
    public float invincibleTime;


    public override bool IsMaxLevel()
    {
        return false;
    }

    public override void Load()
    {
       
    }

    public override void Save()
    {
        
    }

    public override void Updated(Action OnSucces = null, Action OnFailed = null)
    {

    }
}
