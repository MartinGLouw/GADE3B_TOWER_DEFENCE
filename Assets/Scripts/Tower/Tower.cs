using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Tower : MonoBehaviour
{
    public float attackRange = 5.0f;
    public int attackDamage = 50;
    public float attackCooldown = 5f;
    private float lastAttackTime;
    private SphereCollider sphereCollider;
    public int towerHealth = 500;
    public int AttRange = 100;
    private List<IEnemy> enemiesInRange = new List<IEnemy>();
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int Health = 600;
    public int Damage = 30;
    public bool dead;
    public GameManager gameManager;
    public GameObject endScreenCanvas;
    
   
    
    void Start()
    {
        dead = false;
        gameManager = FindObjectOfType<GameManager>();
        endScreenCanvas.SetActive(false);
        StartCoroutine(TowerDefense());
        
    }

    
    

    private IEnumerator TowerDefense()
    {
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                GameObject[] enemiesInRange = enemies
                    .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= AttRange)
                    .ToArray();

                if (enemiesInRange.Length > 0)
                {
                    GameObject closestEnemyInRange = enemiesInRange
                        .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                        .FirstOrDefault();

                    if (closestEnemyInRange != null)
                    {
                        Console.WriteLine("Raptor attacks!"); //Updated message
                        LaunchProjectile(closestEnemyInRange); //Launch projectile
                        yield return new WaitForSeconds(attackCooldown);
                    }
                }
            }

            yield return null;
        }
    }
    private void LaunchProjectile(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

        if (projectileRigidbody != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            projectileRigidbody.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab is missing a Rigidbody component!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
        if (other.gameObject.CompareTag("EP"))
        {
            if(Health > 0)
            {
                Health -= Damage;
                gameManager.UpdateTowerHealthText();
            }
            else
            {
               
                Destroy(gameObject);
                
            }
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
        gameManager.UpdateTowerHealthText();
        CheckDeath();
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

    void CheckDeath()
    {
        if (Health == 0)
        {
            dead = true;
            endScreenCanvas.SetActive(true);
          
        }
    }
    
   
    
}
