using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : Actor
{
    private Player m_player;
    [Header("Enemy Settings")]
    [SerializeField] private float m_attackRange;
    [SerializeField] private float m_attackCooldown;
    [SerializeField] private float m_moveSpeed;

    [Header("Loot Settings")]
    [SerializeField] private LootItem[] lootTable;

    private float currentDamage;
    public float CurrentDamage { get => currentDamage; set => currentDamage = value; }

    private float xpBonus;
    private Transform targetPlayer;
    private float lastAttackTime;
    private EnemyStats m_enemyStats;

    public override void Init()
    {
        base.Init();
        m_player = GameManager.Ins.Player;
        
        if (stastData is EnemyStats enemyStats)
        {
            m_enemyStats = enemyStats;
           
            m_enemyStats.Load();
        }

        if (m_player != null)
        {
            targetPlayer = m_player.transform;
        }
        m_moveSpeed = m_enemyStats.moveSpeed;
        lastAttackTime = 0;
        StatsCaculate();
        OnDead.AddListener(() => OnSpawnCollectable());
        OnDead.AddListener( OnAddXp);
    }

    private void StatsCaculate()
    {
        var playerStats = m_player.PlayerStats;
        if (playerStats == null) return;

        float hpUp = m_enemyStats.hp * Helper.upGrade(playerStats.level + 1);
        float dmgUp = m_enemyStats.damageUp * Helper.upGrade(playerStats.level + 1);

        CurrentHP = m_enemyStats.hp + hpUp;
        currentDamage = m_enemyStats.damage + dmgUp;

        float RandomxpBonus = Random.Range(m_enemyStats.minXPBonus, m_enemyStats.maxXPBonus);
        xpBonus = RandomxpBonus * Helper.upGrade(playerStats.level + 1);

    }
    private void OnSpawnCollectable()
    {
        Vector3 spawnPosition = transform.position;
        foreach (LootItem item in lootTable)
        {
            float dropRoll = Random.Range(0f, 100f);
            if (dropRoll <= item.dropChance)
            {
                Instantiate(item.itemPrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
    private void OnDisable()
    {
        OnDead.RemoveListener(OnSpawnCollectable);
        OnDead.RemoveListener(OnAddXp);
    }
    private void OnAddXp()
    {
        if (m_player != null)
        { 
            m_player.Addexp(xpBonus);
            Debug.Log("+ exp : " + xpBonus);
        }
    }

    protected override void Move()
    {
        if (IsDead || targetPlayer == null) return;

        
        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        m_Rigidbody2.velocity = direction * m_moveSpeed;
        

        float distanceToTarget = Vector2.Distance(transform.position, targetPlayer.position);
        if (distanceToTarget <= m_attackRange)
        {
            TryAttack();
            if (!m_isKnockBack)
            {
                m_Rigidbody2.velocity = Vector2.zero;
                
            }
            else
            {
                
            }

        }
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= m_attackCooldown)
        {
            // m_animation.SetTrigger(AnimationConsts.ENEMY_RUN_PARAM);
            lastAttackTime = Time.time;

            Player player = targetPlayer.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(m_enemyStats.damage);
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (IsDead)
        {
            Dead();
        }

    }
    /*
    private IEnumerator HandleKnocback()
    {
        Vector2 knockbackDirection = (transform.position - targetPlayer.position);
        knockbackDirection.Normalize();
        float initialSpeed = m_moveSpeed;
        float knockbackSpeed = m_moveSpeed * m_enemyStats.knockBackForce;
        float elapsedTime = 0f;

        m_isKnockBack = true;
        while (elapsedTime < m_enemyStats.knockBackTime)   
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / m_enemyStats.knockBackTime;
            float currentSpeed = Mathf.Lerp(knockbackSpeed,0,t);
            m_Rigidbody2.velocity = knockbackDirection * currentSpeed;
            yield return null;
        }
        elapsedTime = 0f;
        while (elapsedTime >= m_enemyStats.knockBackTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / m_enemyStats.knockBackTime;
            float currentSpeed = Mathf.Lerp(0, initialSpeed, t);
            m_Rigidbody2.velocity = knockbackDirection * -1;
            yield return null;
        }

        m_isKnockBack = false ;
    }
    */

    protected override void Dead()
    {
        base.Dead();
        m_animation.SetTrigger(AnimationConsts.ENEMY_RUN_PARAM);
    }
    private void DieEffect()
    {
        Debug.Log("Enemy died!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_attackRange);
    }
}
