using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackRange = 5.0f;
    public int attackDamage = 50;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;
    public Vector3 towerPosition;

    private List<IEnemy> enemiesInRange = new List<IEnemy>();

    void Start()
    {
        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = attackRange;
        towerPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    void Update()
    {
        if (enemiesInRange.Count > 0 && Time.time > lastAttackTime + attackCooldown)
        {
            Attack(enemiesInRange[0]);
            lastAttackTime = Time.time;
        }
    }

    void Attack(IEnemy enemy)
    {
        Debug.Log("Tower attacks!");
        enemy.Health -= attackDamage;
    }
}
