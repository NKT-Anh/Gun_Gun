using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stats : ScriptableObject
{
    public abstract void Save();
    public abstract void Load();
    public abstract void Updated(Action OnSucces = null , Action OnFailed = null);
    public abstract bool IsMaxLevel();

}
