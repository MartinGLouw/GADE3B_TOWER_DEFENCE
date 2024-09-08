using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class ClubCaveman : Enemy
{
    public ClubCaveman()
    {
        Health = 100;
        Damage = 30;
        AttRange = 40;
    }

    public GameObject projectilePrefab; 
    public float projectileSpeed = 10f;

    public override void Attack()
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        StartCoroutine(AttackCoroutine());
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
                        yield return new WaitForSeconds(1f); 
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
}