using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackRange = 5.0f;
    public int attackDamage = 50;
    public float attackCooldown = 1.0f;
    private float lastAttackTime;
    private SphereCollider spherCollider;

    private List<IEnemy> enemiesInRange = new List<IEnemy>();

    void Start()
    {
        
//        spherCollider.isTrigger = true;
//        spherCollider.radius = attackRange;
        
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
