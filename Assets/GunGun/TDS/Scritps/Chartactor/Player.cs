using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Actor
{
    [Header("Player")]
    [SerializeField] private float m_aSpeed;
    [SerializeField] private float m_maxMousePosDistance;
    [SerializeField] private Vector2 m_veclocityLimit;

    [SerializeField] private float m_enemyDection;
    [SerializeField] private LayerMask m_enemyDetectionLayer;

    private Vector2 m_enemyDirectio;

    private float m_currentSpeed;
    private Actor targetEnemy;
    private PlayerStats m_playerStats;
    [Header("even")]
    public UnityEvent OnAddXp;
    public UnityEvent OnLevelUp;
    public UnityEvent OnLostLife;
    public PlayerStats PlayerStats { get => m_playerStats; set => m_playerStats = value; }
    public override void Init()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if(stastData == null)
        {
            return;
        }
        m_playerStats = (PlayerStats)stastData;
        m_playerStats.Load();
        CurrentHP = m_playerStats.hp;
    }
    private void Update()
    {
        Move();
    }
    private void FixedUpdate()
    {
        DetecEnemy();
    }

    private void DetecEnemy()
    {
        var enemtFineds = Physics2D.OverlapCircleAll(transform.position, m_enemyDection, m_enemyDetectionLayer);
        var finalEnemy = FindNearestEnemy(enemtFineds);
        if (finalEnemy == null) return;
        targetEnemy = finalEnemy;
        WeaponHandl();
    }

    private void WeaponHandl()
    {
        if(targetEnemy == null || weapon == null) return;
        m_enemyDirectio = targetEnemy.transform.position - weapon.transform.position;
        m_enemyDirectio.Normalize();
        float angle = Mathf.Atan2(m_enemyDirectio.y, m_enemyDirectio.x) * Mathf.Rad2Deg;
        weapon.transform.rotation =  Quaternion.Euler(0f, 0f, angle) ;
        if (m_isKnockBack)
        {
            
            return ;
        }
        else
        {
            weapon.Shot(m_enemyDirectio);
        }


    }

    private Actor FindNearestEnemy(Collider2D[] enemtFineds)
    {
        
        float minDistance = Mathf.Infinity;
        Actor nearestEnemy = null;
        if (enemtFineds == null || enemtFineds.Length <= 0) return null;
        foreach(var enemyfind in enemtFineds)
        {
            Actor emeny  = enemyfind.GetComponent<Actor>();
            if (emeny == null || emeny.IsDead) continue;
            float distance  = Vector2.Distance(transform.position, emeny.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = emeny;
            }
        }
        return nearestEnemy;
    }

    protected override void Move()
    {
        if (IsDead) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 movingDirection = mousePos  - (Vector2)transform.position;
        movingDirection.Normalize();
        if (!m_isKnockBack) 
        {
            if (Input.GetMouseButton(0))
            {
                Run(mousePos, movingDirection);
            }
            else 
            {
                BackToIdel();
            }
            return; 
        }
        m_Rigidbody2.velocity = m_enemyDirectio * -stastData.knockBackForce * Time.deltaTime;
        m_animation.SetBool(AnimationConsts.PLAYER_RUN_PARAM, false);

    }

    private void BackToIdel()
    {
        m_currentSpeed -= m_aSpeed * Time.deltaTime;
        m_currentSpeed = Mathf.Clamp(m_currentSpeed,0, m_currentSpeed);

        m_Rigidbody2.velocity = Vector2.zero;
        m_animation.SetBool(AnimationConsts.PLAYER_RUN_PARAM, false);
    }

    private void Run(Vector2 mousePos, Vector2 movingDirection)
    {
        m_currentSpeed += m_aSpeed * Time.deltaTime;
        m_currentSpeed = Mathf.Clamp(m_currentSpeed, 0, m_playerStats.moveSpeed);
        float delta = m_currentSpeed * Time.deltaTime;
        float distanToMousePos = Vector2.Distance(transform.position, mousePos);
        distanToMousePos = Mathf.Clamp(distanToMousePos,0, m_maxMousePosDistance / 3);
        delta *= distanToMousePos;

        m_Rigidbody2.velocity  = movingDirection * delta;

        float m_veclocityLimitX = Mathf.Clamp(m_Rigidbody2.velocity.x, -m_veclocityLimit.x, m_veclocityLimit.y);
        float m_veclocityLimitY = Mathf.Clamp(m_Rigidbody2.velocity.y, -m_veclocityLimit.y, m_veclocityLimit.y);
        m_Rigidbody2.velocity = new Vector2(m_veclocityLimitX,m_veclocityLimitY);

        m_animation.SetBool(AnimationConsts.PLAYER_RUN_PARAM, true);
    }
    public void Addexp(float xpBonus)
    {
        if (m_playerStats == null) return;
        m_playerStats.xp += xpBonus;
        m_playerStats.Updated(OnUpgradeStats);
        OnAddXp?.Invoke();
        m_playerStats.Save();
    }
    private void OnUpgradeStats()
    {
        OnLevelUp?.Invoke();

    }
    public override void TakeDamage(float damage)
    {
        if(damage <=0 || m_isInviciable)
            return;
        CurrentHP -=damage;
        CurrentHP = Mathf.Clamp(CurrentHP,0,PlayerStats.hp);
        KnocBack();
        OnTakeDamage?.Invoke();
        if(CurrentHP > 0) return;
        GameManager.Ins.GameOverCheck(OnLostLifeDelegate, OnDeadDelegate);
    }
    private void OnLostLifeDelegate()
    {
        CurrentHP = m_playerStats.hp;

        if (m_isKnockBackCoroutine != null || m_isInviciableCoroutine != null)
        {
            return;
        }

        Invinsible(3.5f);
        
        OnLostLife?.Invoke();
    }
    private void OnDeadDelegate()
    {
        CurrentHP = 0;
        Dead();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy ememy = collision.gameObject.GetComponent<Enemy>();
            if (ememy != null)
            {
                
            }
        }    
        else if(collision.gameObject.CompareTag(TagConsts.COLLETABLE_TAG)){
            Item colectable = collision.gameObject.GetComponent<Item>();
            colectable?.Trigger();
            Destroy(colectable.gameObject);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_enemyDection);
    }
}
