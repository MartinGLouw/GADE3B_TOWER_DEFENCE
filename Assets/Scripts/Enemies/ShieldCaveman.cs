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
        AttRange = 120; 
    }

    public int damage = 20;
    public GameObject shieldProjectilePrefab;
    public float projectileSpeed = 15f;
    public float attackCooldown = 3f;
    public MeatManager meatManager;
    public Slider healthSliderShield;
    private EnemProjectile _enemProjectile;
    private DefenProjectile _defenProjectile;

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    private void Start()
    {
        healthSliderShield.maxValue = Health;
        healthSliderShield.minValue = 0;
        healthSliderShield.value = Health;
        StartCoroutine(ShieldThrowCoroutine());
    }

    private void Update()
    {
        if (healthSliderShield != null)
        {
            healthSliderShield.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
            healthSliderShield.value = Health; 
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
        }
        else
        {
            Debug.LogError("Shield projectile prefab is missing a Rigidbody component!");
        }
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
            if(Health > 0)
            {
                Health -= _defenProjectile.damage;
                healthSliderShield.value = Health;
            }
            else if (Health == 0)
            {
               
                Destroy(gameObject);
                MeatIncrease();
            }
        }
    }
}
