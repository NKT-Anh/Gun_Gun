using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Stats", menuName = "Stats/PlayerStats")]
public class PlayerStats : ActorStats
{
   
    [Header("Level Up Base")]
    public int level;
    public int maxLevel;
    public float xp;
    public float leverUpXP;
    [Header("LevelUp")]
    public float xpUp;
    public float hpUp;
    public float damageUp;
    private const string PlayerStatsKey = "PlayerStats";

    public override bool IsMaxLevel()
    {
        return level >= maxLevel;
    }
    public override void Load()
    {
        string json = PlayerPrefs.GetString(PlayerStatsKey,"{}");
        if (!string.IsNullOrEmpty(json)){
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
    public override void Save()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PlayerStatsKey, json);
        PlayerPrefs.Save();
    }
    public override void Updated(Action OnSucces = null, Action OnFailed = null)
    {
       
        while (xp >= leverUpXP && !IsMaxLevel())
        {
            level++;
            xp -= xpUp;

            hp += hpUp * Helper.upGrade(level); 
            leverUpXP += xpUp * Helper.upGrade(level);
            Save();
            OnSucces?.Invoke();
        }
        if(xp < leverUpXP && IsMaxLevel())
        {
            OnFailed?.Invoke();
        }
    }

}
