using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunStatDialog : Dialog
{
    [SerializeField] private GunStatUI BulletStat_data;
    [SerializeField] private GunStatUI DamageStat_data;
    [SerializeField] private GunStatUI FirerateStat_data;
    [SerializeField] private GunStatUI ReloadTimeStat_data;
    [SerializeField] private TextMeshProUGUI UpgradeTxt;
    private Weapon weapon;
    private WeaponStats weaponStats;
    public override void Show(bool isShow)
    {
        base.Show(isShow);

        Time.timeScale = 0f;

        weapon = GameManager.Ins.Player.weapon;
        weaponStats = weapon.Stats;
        if (weapon == null)
        {
            Debug.LogError("!!!");
            return;
        }
        ShowGunStats();

    }

    private void ShowGunStats()
    {
        
        titleTxt.text = $"{weapon.Stats.WeaponName} LEVEL:{weaponStats.level.ToString("00") }";

        if (UpgradeTxt != null) 
        {
            UpgradeTxt.text = $"UP [${weaponStats.upgradePrice.ToString("n0")}]";
        }

        if(weapon == null) return;
        if (BulletStat_data != null)
        {
            
            BulletStat_data.UpdateGun(
                "Bullets : ",
                weaponStats.bullets.ToString("n0"),
                $"(+ {weaponStats.bulletsUP.ToString("n0")})"
            );
        }
        DamageStat_data?.UpdateGun(
            label: "Damage : ",
            value: weaponStats.bulletDamage.ToString("F2"),
            updateValue: $"(+ {weaponStats.bulletDamageUp.ToString("F3")})"

            );
        FirerateStat_data?.UpdateGun(
            label: "Firerate : ",
            value: weaponStats.firerate.ToString("F2"),
            updateValue: $"(- {weaponStats.firerateUp.ToString("F3")})"

            );
        ReloadTimeStat_data?.UpdateGun(
            label: "Reload : ",
            value: weaponStats.reloadTime.ToString("F2"),
            updateValue: $"(- {weaponStats.reloadTimeUp.ToString("F3")})"

            );

    }

    public void UpgradeGun()
    {
        if (weapon == null) return;
        if(Prefs.coins >= weapon.Stats.upgradePrice)
        {
            weaponStats.Updated(UpdateUIGun, OnFailed);

        }
        else
        {
            Debug.Log("Not enough coins to upgrade!");
        }

    }
    private void UpdateUIGun()
    {
        ShowGunStats();
        GUIManager.Ins.ShowCoinGUI(Prefs.coins);
        AudioController.Ins.PlaySound(AudioController.Ins.upgradeSuccess);
    }
    private void OnFailed()
    {
        Debug.Log(":)))");
    }    
    public override void Close()
    {
        base.Close();
        Time.timeScale = 1f;
    }
}
