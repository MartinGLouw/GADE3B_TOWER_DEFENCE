using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClubCaveman : Enemy
{
    public ClubCaveman()
    {
        //Health = 100;
        Damage = 30;
        AttRange = 120;
    }
    public int damage = 50;
    public int health = 100;
    public GameObject projectilePrefab; 
    public float projectileSpeed = 10f;
    public float attackCooldown = 2f;
    public MeatManager meatManager;
    public Slider healthSlider;
    public override void Attack()
    { 
        throw new NotImplementedException();
    }
    void Update()
    {
        if (Health <= 0)
        {
            meatManager.meat += 30;
            meatManager.UpdateMeatText();
        }
    }

    private void Start()
    {
        healthSlider.maxValue = health; 
        healthSlider.minValue = 0;  
        healthSlider.value = health;
        StartCoroutine(AttackCoroutine());
    }

    private void MeatIncrease()
    {
        meatManager.meat = meatManager.meat + 20;
    }

    private IEnumerator AttackCoroutine()
    {
        while (true) 
        {
            GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");

            if (defenders.Length > 0)
            {
                GameObject[] defendersInRange = defenders
                    .Where(defender => Vector3.Distance(transform.position, defender.transform.position) <= AttRange)
                    .ToArray();

                if (defendersInRange.Length > 0)
                {
                    GameObject closestDefenderInRange = defendersInRange
                        .OrderBy(defender => Vector3.Distance(transform.position, defender.transform.position))
                        .FirstOrDefault();

                    if (closestDefenderInRange != null)
                    {
                        Console.WriteLine("ClubCaveman attacks!");
                        LaunchProjectile(closestDefenderInRange);
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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DP"))
        {
            if(health > 0)
            {
                health -= damage;
                healthSlider.value = health;
            }
            else if (health == 0)
            {
               
                Destroy(gameObject);
                MeatIncrease();
            }
        }
    }
    
}