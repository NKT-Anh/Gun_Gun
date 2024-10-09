using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("stats")]
    public WeaponStats Stats;
    [SerializeField] private Transform m_shotingPoint;
    [SerializeField] private GameObject m_bulletPrefabs;
    [SerializeField] private GameObject m_muzzFlatsPrefab;
    private float m_curFR;
    private int m_currentBullet;
    private float m_currentReloadTime;

    private bool m_isShoted;
    private bool m_isReloating;

    [Header("even")]
    public UnityEvent OnShot;
    public UnityEvent OnReload;
    public UnityEvent OnReloadDone;
    private void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if (Stats == null)
        {
            return;
        }
        Stats.Load();
        m_curFR = Stats.firerate;
        m_currentReloadTime =Stats.reloadTime;
        m_currentBullet = Stats.bullets;

    }
    private void Update()
    {
        ReduceFirerate();
        ReduceReloadtime();
    }

    private void ReduceFirerate()
    {
        if(!m_isShoted) {return;}
        m_curFR -= Time.deltaTime;
        if (m_curFR > 0) { return; }
        else { m_curFR = Stats.firerate; }
        m_isShoted = false;


    }

    private void ReduceReloadtime()
    {
        if(!m_isReloating)return;
        m_currentReloadTime -=  Time.deltaTime;
        if(m_currentReloadTime > 0) { return; }
        LoadStats();
        OnReloadDone?.Invoke();
    }
    public void Shot(Vector3 tagerDirection)
    {
        if (m_isShoted || m_shotingPoint == null || m_currentBullet <= 0) return;
        if (m_muzzFlatsPrefab)
        {
            var muzzFlatsClon =Instantiate(m_muzzFlatsPrefab, m_shotingPoint.position,transform.rotation);
            muzzFlatsClon.transform.SetParent(m_shotingPoint);
        }
        if(m_bulletPrefabs)
        {
            var bulletClon = Instantiate(m_bulletPrefabs, m_shotingPoint.position, transform.rotation);
            var projecttitleComp = bulletClon.GetComponent<ProjectTitle>();
            if (projecttitleComp != null) 
            {
                projecttitleComp.Damage = Stats.bulletDamage;
            }
        }
        m_currentBullet--;
        m_isShoted = true;
        if(m_currentBullet <=0)
        {
            ReLoad();   
        }
        
        OnShot?.Invoke();
    }
    public void ReLoad()
    {
        m_isReloating = true;
        OnReload?.Invoke();

    }
}
