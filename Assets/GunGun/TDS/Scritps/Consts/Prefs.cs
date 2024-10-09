using System;
using UnityEngine;

public static class Prefs
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int level;
        public int health;
    }
    [Serializable]
    public class EnemyData
    {
        public string enemyName;
        public int level;
        public int health;
    }
    [Serializable]
    public class WeaponData
    {
        public string weaponName;
        public int damage;
        public int range;
    }
    public static int coins
    {
        set => PlayerPrefs.SetInt(PrefConsts.COIN_KEY, value);
        get=> PlayerPrefs.GetInt(PrefConsts.COIN_KEY);
    }
    
     public static PlayerData playerData
    {
        set
        {
            try
            {
                string json = JsonUtility.ToJson(value);
                
                PlayerPrefs.SetString(PrefConsts.PLAYER_DATA_KEY, json);
                PlayerPrefs.Save();
            }
            catch(Exception ex) 
            {
                Debug.Log($"Loi truy xuat player:{ex.Message}");
            }
        }
        get
        {
            string json = PlayerPrefs.GetString(PrefConsts.PLAYER_DATA_KEY);
            return JsonUtility.FromJson<PlayerData>(json);
        }
    }

    public static EnemyData enemyData
    {
        set
        {
            string json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(PrefConsts.ENEMY_DATA_KEY, json);
        }
        get
        {
            string json = PlayerPrefs.GetString(PrefConsts.ENEMY_DATA_KEY);
            return JsonUtility.FromJson<EnemyData>(json);
        }
    }

    public static WeaponData weaponData
    {
        set
        {
            string json = JsonUtility.ToJson(value);
            PlayerPrefs.SetString(PrefConsts.WEAPON_DATA_KEY, json);
        }
        get
        {
            string json = PlayerPrefs.GetString(PrefConsts.WEAPON_DATA_KEY);
            return JsonUtility.FromJson<WeaponData>(json);
        }
    }
    
    public static bool IsEnoughtCoin(int cointToCheck)
    {
        return coins >= cointToCheck;
    }
}
