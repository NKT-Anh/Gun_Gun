using System;
using System.Collections;
using UDEV;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))] 
public class Actor : MonoBehaviour
{
    [Header("Head")]
    
    public ActorStats stastData;
    [LayerList]
    [SerializeField] private int m_inviciablePlayer;
    [LayerList]
    [SerializeField] private int m_normalPlayer;

    public Weapon weapon;
    protected bool m_isKnockBack;
    protected bool m_isInviciable;
    private bool m_isDead;
    private float currentHP;
    

    protected Rigidbody2D m_Rigidbody2;
    protected Animator m_animation;

    protected Coroutine m_isInviciableCoroutine;
    protected Coroutine m_isKnockBackCoroutine;


    [Header("Event")]
    public UnityEvent OnInit;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDead;

    public float CurrentHP 
    { 
        get => currentHP;
        set => currentHP = value;
    }

    public bool IsDead { get => m_isDead; set => m_isDead = value; }
    

    public virtual void Awake()
    {
        m_Rigidbody2 = GetComponent<Rigidbody2D>();
        if (m_Rigidbody2 == null)
            Debug.LogError("Khong co rigidbody");

        m_animation = GetComponentInChildren<Animator>();
        if (m_animation == null)
            Debug.LogError("khong co animator");


    }
    public virtual void Start()
    {
        Init();
        OnInit?.Invoke();   
    }
    public virtual void Init()
    {

    }
    public virtual void TakeDamage(float damage)
    {
        if(damage <0 || m_isInviciable)
        {
            return;
        }
        currentHP -= damage;
        KnocBack();
        if(currentHP <= 0)
        {
            currentHP = 0;
            Dead();
        }
        else
        {
            OnTakeDamage?.Invoke();
        }
    }

    protected virtual void Dead()
    {
        IsDead = true;
        m_Rigidbody2.velocity  = Vector3.zero;
        OnDead?.Invoke();
        Destroy(gameObject, 0.5f);
    }

    protected void KnocBack()
    {
        if (m_isDead || m_isKnockBack || m_isInviciable) return;
        m_isKnockBack = true;
        m_isKnockBackCoroutine = StartCoroutine(BlockKnocBack());

    }
    private IEnumerator BlockKnocBack()
    {
        yield return new WaitForSeconds(stastData.knockBackTime);
        Invinsible(stastData.invincibleTime);

    }
    protected void Invinsible(float time)
    {
        m_isKnockBack = false;
        m_isInviciable = true;
        gameObject.layer = m_inviciablePlayer;
        m_isInviciableCoroutine = StartCoroutine(StopInviciablePlayer(time));
    }    

    private IEnumerator StopInviciablePlayer(float time)
    {
        yield return new WaitForSeconds(time);
        m_isInviciable = false;
        gameObject.layer = m_normalPlayer;
    }
    protected virtual void Move()
    {
        
    }
    
}
