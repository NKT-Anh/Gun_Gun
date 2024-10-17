using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int m_minBonus;
    [SerializeField] private int m_maxBonus;
    [SerializeField] private int m_lineTime;
    [SerializeField] private int m_spawnForce;
    private int m_lifeTimeCounting;
    private Rigidbody2D m_rb;
    private FlashVfx m_FlashVfx;

    protected int m_Bonus;
    protected Player m_Player;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_FlashVfx = GetComponent<FlashVfx>();
      
    }
    private void Start()
    {
        m_lifeTimeCounting = m_lineTime;
        m_Player = GameManager.Ins.Player;
        m_Bonus = Random.Range(m_minBonus, m_maxBonus) * m_Player.PlayerStats.level;

        Inti();
        Explode();
        FlashVfxComleted();
        StartCoroutine(CountDown());

    }

    private IEnumerator CountDown()
    {
        
        while (m_lifeTimeCounting > 0)
        {
            float timeLifeLeftRate = Mathf.Round((float)m_lifeTimeCounting / m_lineTime);
            yield return new WaitForSeconds(1f);
            m_lifeTimeCounting--;
            if(timeLifeLeftRate <= 0.3 && m_FlashVfx != null)
            {
                m_FlashVfx.Flash(m_lifeTimeCounting);
            }
        }
        yield return null;
    }

    private void FlashVfxComleted()
    {
        if(m_FlashVfx == null)  return;
        m_FlashVfx.OnCompleted.RemoveAllListeners();   
        m_FlashVfx.OnCompleted.AddListener(() =>DestroyCollectabel());   

    }
    private void DestroyCollectabel()
    {
        Destroy(gameObject);
    }


    private void Explode()
    {
        if (m_rb == null) return;
        float randomForceX = Random.Range(-m_spawnForce, m_spawnForce);
        float randomForceY = Random.Range(-m_spawnForce, m_spawnForce);
        m_rb.velocity =  new Vector2 (randomForceX, randomForceY) *Time.deltaTime;
        StartCoroutine(StopMoving() );
    }

    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.8f);
        if(m_rb != null) 
            {
            m_rb.velocity = Vector2.zero;
            }
    }

    public virtual void Inti()
    {
       
    }
    public virtual void Trigger()
    {

    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
