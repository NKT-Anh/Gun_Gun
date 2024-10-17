using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    public WeaponStats Stats;

    [SerializeField] private Transform m_shotingPoint;
    [SerializeField] private GameObject m_bulletPrefabs;
    [SerializeField] private GameObject m_muzzFlatsPrefab;

    private float m_curFR;
    private int m_currentBullet;
    private float m_currentReloadTime;
    private bool m_isShoted;
    private bool m_isReloating;

    [Header("Events")]
    public UnityEvent OnShot;
    public UnityEvent OnReload;
    public UnityEvent OnReloadDone;

    private void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if (Stats == null) return;

        Stats.Load();
        m_curFR = Stats.firerate;
        m_currentReloadTime = Stats.reloadTime;
        m_currentBullet = Stats.bullets;
    }

    private void Update()
    {
        ReduceFirerate();
        ReduceReloadtime();
    }

    private void ReduceFirerate()
    {
        if (!m_isShoted) return;

        m_curFR -= Time.deltaTime;
        if (m_curFR > 0) return;

        m_curFR = Stats.firerate;
        m_isShoted = false;
    }

    private void ReduceReloadtime()
    {
        if (!m_isReloating) return;

        m_currentReloadTime -= Time.deltaTime;
        if (m_currentReloadTime > 0) return;

        LoadStats();
        m_isReloating = false;
        OnReloadDone?.Invoke();
    }

    public void Shot(Vector3 targetDirection)
    {
        if (m_isShoted || m_shotingPoint == null || m_currentBullet <= 0) return;

        if (m_muzzFlatsPrefab)
        {
            var muzzFlatsClone = Instantiate(m_muzzFlatsPrefab, m_shotingPoint.position, transform.rotation);
            muzzFlatsClone.transform.SetParent(m_shotingPoint);
        }

        if (m_bulletPrefabs)
        {
            var bulletClone = Instantiate(m_bulletPrefabs, m_shotingPoint.position, transform.rotation);
            var projectTitleComp = bulletClone.GetComponent<ProjectTitle>();
            if (projectTitleComp != null)
            {
                projectTitleComp.Damage = Stats.bulletDamage;
            }
        }

        m_currentBullet--;
        m_isShoted = true;

        if (m_currentBullet <= 0)
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
