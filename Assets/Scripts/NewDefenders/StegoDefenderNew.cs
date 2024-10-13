using System.Collections;
using System.Linq;
using UnityEngine;

public class StegoDefenderNew : DefenderNew
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f; 
    public int damage = 10; 
    public int projectilesCount = 3; 
    public float attackCooldown = 1.5f; 
    public int meatCost = 30;

    private void Start()
    {
        Health = 80; 
        Damage = damage; 
        MeatCost = meatCost; 
        StartCoroutine(DefendCoroutine());
    }

    public override IEnumerator DefendCoroutine()
    {
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                GameObject[] enemiesInRange = enemies
                    .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= AttackRange)
                    .ToArray();

                if (enemiesInRange.Length > 0)
                {
                    for (int i = 0; i < projectilesCount; i++)
                    {
                        GameObject closestEnemyInRange = enemiesInRange[0]; 
                        LaunchProjectile(closestEnemyInRange);
                    }
                    yield return new WaitForSeconds(attackCooldown); 
                }
            }
            yield return null; 
        }
    }

    protected override void LaunchProjectile(GameObject target)
    {
        Debug.Log($"{this.name} is launching a projectile at {target.name}.");
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        StegoProjectileNew stegoProjectile = projectile.GetComponent<StegoProjectileNew>();

        if (stegoProjectile != null)
        {
            stegoProjectile.Initialize(target, projectileSpeed, damage); 
        }
        else
        {
            Debug.LogError("Stego projectile prefab is missing the StegoProjectileNew component!");
        }
    }
}
