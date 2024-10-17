using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Stats/WeaponStats")]
public class WeaponStats : Stats
{
    [Header("Base Stats")]
    public string WeaponName;
    public int bullets;
    public float bulletSpeed;
    public float bulletDamage;
    public float reloadTime;
    public float firerate;
    public int upgradePrice;
    [Header("Up Grade")]
    public int level;
    public int maxlevel;
    public int bulletsUP;
    public float bulletDamageUp;
    public float reloadTimeUp;
    public float firerateUp;
    public int upgradePriceUP;
    [Header("Limit")]
    public float minFirerate = 0.1f;
    public float minReloadTime = 0.1f;
    public int bulletsUPInfo { get => bulletsUP * (level + 1); }
    public float bulletDamageUpInfo { get => bulletDamageUp * Helper.upGrade(level + 1); }
    public float reloadTimeUpInfo { get => reloadTimeUp * Helper.upGrade(level + 1); }
    public float firerateUpInfo { get => firerateUp * Helper.upGrade(level + 1); }
    public int upgradePriceUPInfo { get => upgradePriceUP; }
    private const string WeaponStatsKey = "WeaponStat";
    public override bool IsMaxLevel()
    {
        return level >= maxlevel;
    }
    public override void Load()
    {
        string json = PlayerPrefs.GetString(WeaponStatsKey, "{}");
        if (!string.IsNullOrEmpty(json))
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }
    public override void Save()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(WeaponStatsKey, json);
        PlayerPrefs.Save();
    }

    public override void Updated(Action OnSucces = null, Action OnFailed = null)
    {
        if(Prefs.coins >= upgradePrice && !IsMaxLevel())
        {
            Prefs.coins -= upgradePrice;
            level++;
            bullets += bulletsUP * level;

            firerate -= firerateUp * Helper.upGrade(level);
            firerate = Mathf.Clamp(firerate, minFirerate, firerate);

            reloadTime -= reloadTimeUp * Helper.upGrade(level);
            reloadTime = Mathf.Clamp(reloadTime, minReloadTime, reloadTime);

            bulletDamage += bulletDamageUp * Helper.upGrade(level);
            upgradePrice += upgradePriceUP * level;

            Save();
            OnSucces?.Invoke();
            return;
        }
        OnFailed?.Invoke();
    }
}
