using System.Collections;
using System.Linq;
using UnityEngine;

public class RaptorDefenderNew : DefenderNew
{
    public GameObject projectilePrefab; 
    public float projectileSpeed = 15f; 
    public float attackCooldown = 2f; 
    public int meatCost = 20;
    public int damage = 30;

    private void Start()
    {
        Health = 100;
        Damage = damage; 
        MeatCost = meatCost; 
        StartCoroutine(DefendCoroutine());
    }

    public override IEnumerator DefendCoroutine()
    {
        Debug.Log($"{this.name} started defending.");
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log($"Enemies detected: {enemies.Length}");

            if (enemies.Length > 0)
            {
                GameObject[] enemiesInRange = enemies
                    .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= AttackRange)
                    .ToArray();
                Debug.Log($"Enemies in range: {enemiesInRange.Length}");

                if (enemiesInRange.Length > 0)
                {
                    GameObject closestEnemyInRange = enemiesInRange
                        .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                        .FirstOrDefault();

                    if (closestEnemyInRange != null && canShoot)
                    {
                        Debug.Log($"{this.name} found enemy {closestEnemyInRange.name} in range.");
                        LaunchProjectile(closestEnemyInRange);
                        yield return new WaitForSeconds(attackCooldown); 
                    }
                }
            }

            yield return null; 
        }
    }

    protected override void LaunchProjectile(GameObject target)
    {
        Debug.Log($"{this.name} is launching a projectile at {target.name}.");
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        RaptorProjectileNew raptorProjectile = projectile.GetComponent<RaptorProjectileNew>();

        if (raptorProjectile != null)
        {
            raptorProjectile.Initialize(target, projectileSpeed, damage); 
        }
    }
}
