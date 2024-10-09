using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTitle : MonoBehaviour
{
    [Header("Base")]
    [SerializeField]  private float speed;
    private float damage;
    private float m_curSpeed;
    [SerializeField] private GameObject m_bodyHitPrefab;
    private Vector2 lastPos;
    private RaycastHit2D m_RaycastHit;

    public float Damage { get => damage; set => damage = value; }

    private void Start()
    {
        m_curSpeed = speed;
        RefeslastPos();
    }

    private void RefeslastPos()
    {
        lastPos = (Vector2) transform.position;
    }
    private void Update()
    {
        transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        DeadDamage();
        RefeslastPos();
    }

    private void DeadDamage()
    {
        Vector2 rayDirection = (Vector2)transform.position - lastPos;
        m_RaycastHit = Physics2D.Raycast(lastPos, rayDirection, rayDirection.magnitude);
        var col = m_RaycastHit.collider;
        if(!m_RaycastHit || col == null)
        {
            return;
        }
        if (col.CompareTag(TagConsts.ENEMY_TAG)){
            DeadDamagetoEnemy(col);
        }
    }

    private void DeadDamagetoEnemy(Collider2D collider)
    {
        Actor actor = collider.GetComponent<Actor>();
        actor?.TakeDamage(damage);
        if (m_bodyHitPrefab)
        {
            Instantiate(m_bodyHitPrefab, (Vector3)m_RaycastHit.point, Quaternion.identity);
        }
        Destroy(gameObject);

    }
    private void OnDisable()
    {

        m_RaycastHit = new RaycastHit2D();
    }
}
