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

    private void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.minValue = 0;
        healthSlider.value = health;
        StartCoroutine(ShieldThrowCoroutine());
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            meatManager.meat += 50; 
            meatManager.UpdateMeatText();
        }
    }

    private void MeatIncrease()
    {
        meatManager.meat = meatManager.meat + 40; 
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
                        Debug.Log("ShieldCaveman throws a boomerang shield!");
                        StartCoroutine(ThrowBoomerangShield(closestDefenderInRange));
                        yield return new WaitForSeconds(attackCooldown);
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator ThrowBoomerangShield(GameObject target)
    {
        GameObject shieldProjectile = Instantiate(shieldProjectilePrefab, transform.position, Quaternion.identity);
        Rigidbody shieldRigidbody = shieldProjectile.GetComponent<Rigidbody>();
        Vector3 initialPosition = transform.position;

        if (shieldRigidbody != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            bool returning = false;
            
            //outward throw and return
            while (true)
            {
                if (!returning) //Throw the shield outward
                {
                    shieldRigidbody.velocity = direction * projectileSpeed;

                    if (Vector3.Distance(shieldProjectile.transform.position, target.transform.position) < 1f)
                    {
                        //Hit the target defender
                        Debug.Log("Shield hits the defender on the outward path!");
                        DealDamageToDefender(target);
                        returning = true; // Switch to return phase
                    }
                }
                else //Return the shield
                {
                    Vector3 returnDirection = (initialPosition - shieldProjectile.transform.position).normalized;
                    shieldRigidbody.velocity = returnDirection * projectileSpeed;

                    if (Vector3.Distance(shieldProjectile.transform.position, initialPosition) < 1f)
                    {
                        //Return to the ShieldCaveman
                        Debug.Log("Shield returns to ShieldCaveman!");
                        Destroy(shieldProjectile);
                        break;
                    }
                }

                yield return null;
            }
        }
        else
        {
            Debug.LogError("Shield projectile prefab is missing a Rigidbody component!");
        }
    }

   
    private void DealDamageToDefender(GameObject defender)
    {
        Defender defenderScript = defender.GetComponent<Defender>();
        if (defenderScript != null)
        {
            defenderScript.Health -= damage;
            Debug.Log("Dealt " + damage + " damage to defender!");
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
            else if (health == 0)
            {
                Destroy(gameObject);
                MeatIncrease();
            }
        }
    }
}
