using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShieldCaveman : Enemy
{
    public ShieldCaveman()
    {
        Health = 250;  
        Damage = 20;   
        AttRange = 60; 
    }

    public int damage = 20;
    public int health = 250;
    public GameObject shieldProjectilePrefab;
    public float projectileSpeed = 8f;
    public float attackCooldown = 3f;
    public MeatManager meatManager;
    public Slider healthSlider;
    public float explosionRadius = 5f; //AoE radius for shield throw

    private void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.minValue = 0;
        healthSlider.value = health;
        StartCoroutine(ShieldThrowCoroutine());
    }

    public override void Attack()
    {
    }

    private void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
            healthSlider.value = Health; 
        }
        
        if (Health <= 0)
        {
            meatManager.meat += 50;
            meatManager.UpdateMeatText();
        }
    }

    private void MeatIncrease()
    {
        meatManager.meat += 40; 
    }

    private IEnumerator ShieldThrowCoroutine()
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
                        Debug.Log("ShieldCaveman throws a heavy shield!");
                        ThrowShield(closestDefenderInRange);
                        yield return new WaitForSeconds(attackCooldown);
                    }
                }
            }

            yield return null;
        }
    }

    private void ThrowShield(GameObject target)
    {
        GameObject shieldProjectile = Instantiate(shieldProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody shieldRigidbody = shieldProjectile.GetComponent<Rigidbody>();

        if (shieldRigidbody != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            shieldRigidbody.velocity = direction * projectileSpeed;
            StartCoroutine(ReturnShield(shieldProjectile, target));
        }
        else
        {
            Debug.LogError("Shield projectile prefab is missing a Rigidbody component!");
        }
    }

    private IEnumerator ReturnShield(GameObject shieldProjectile, GameObject target)
    {
        yield return new WaitForSeconds(0.5f); 
        
        HitDefender(target);
        
        Destroy(shieldProjectile);
    }

    public void HitDefender(GameObject defender)
    {
        Defender defenderScript = defender.GetComponent<Defender>();

        if (defenderScript != null)
        {
        
            defenderScript.Health -= damage; 
            
            if (defenderScript.Health <= 0)
            {
                Destroy(defender); 
            }
        }
        else
        {
            Debug.LogError("Hit object does not have a Defender component!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DP"))
        {
            if (health > 0)
            {
                health -= damage;
                healthSlider.value = health;
            }
            else if (health <= 0)
            {
                Destroy(gameObject);
                MeatIncrease();
            }
        }
    }
}
