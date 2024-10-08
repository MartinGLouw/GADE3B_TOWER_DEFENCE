using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RaptorDefender : Defender
{
    public GameObject projectilePrefab; 
    public float projectileSpeed = 15f; //Adjust the speed as needed
    public float attackCooldown = 2f;
    public int meatCost = 20;
    public int damage = 30;
    public int health = 100;
    private bool isTakingDamage = false;
    public Slider healthSlider;
    private void Start()
    {
        
        Health = 100;
        Damage = 50;
        AttRange = 120;
        healthSlider.maxValue = Health; 
        healthSlider.minValue = 0; 
        healthSlider.value = Health;
        StartCoroutine(DefendCoroutine());
    }

    
    

    private IEnumerator DefendCoroutine()
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
                        Console.WriteLine("Raptor attacks!");
                        LaunchProjectile(closestEnemyInRange);

                        //Wait for the attack cooldown before attacking again
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

    //You can keep the Defend method for other types of defense if needed
    public override void Defend(IEnemy enemy)
    {
        if (enemy != null)
        {
            Console.WriteLine("Raptor Bites!"); //This might be used for a close-range attack
            enemy.Health -= Damage;
        }
        else
        {
            Debug.LogError("Closest enemy in range does not have an IEnemy component!");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EP") && !isTakingDamage)
        {
            
            isTakingDamage = true; //Set the flag to prevent continuous damage

            if (Health > 0)
            {
                Health -= damage;
                healthSlider.value = Health;
            }
            else 
            {
                Destroy(gameObject);
            }

            StartCoroutine(ResetDamageFlag()); //Reset the flag after a short delay
        }
    }
    private IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.5f); //Adjust delay as needed
        isTakingDamage = false;
    }
    
}
