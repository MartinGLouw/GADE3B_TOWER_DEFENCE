using UnityEngine;
using System.Collections;
using System.Linq;

public class ShieldCavemanNew : EnemyNew
{
    public GameObject shieldProjectilePrefab;

    private void Start()
    {
        Health = 120f;
        Damage = 20f; 
        AttackCooldown = 3f;
        StartCoroutine(AttackCoroutine());
    }

  

    protected override void LaunchProjectile(GameObject target)
    {
        GameObject projectileInstance = Instantiate(shieldProjectilePrefab, transform.position, Quaternion.identity);
        ShieldProjectileNew shieldProjectile = projectileInstance.GetComponent<ShieldProjectileNew>();

        if (shieldProjectile != null)
        {
            shieldProjectile.Initialize(target, 10f, Damage); 
        }
    }

    protected override IEnumerator AttackCoroutine()
    {
        Debug.Log($"{this.name} started Attacking.");
        while (true)
        {
            GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
            Debug.Log($"defender detected: {defenders.Length}");

            if (defenders.Length > 0)
            {
                GameObject[] defendersInRange = defenders
                    .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= AttackRange)
                    .ToArray();
                Debug.Log($"defender in range: {defendersInRange.Length}");

                if (defendersInRange.Length > 0)
                {
                    GameObject closesDefenderInRange = defendersInRange
                        .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                        .FirstOrDefault();

                    if (closesDefenderInRange != null && canShoot)
                    {
                        Debug.Log($"{this.name} found defender {closesDefenderInRange.name} in range.");
                        LaunchProjectile(closesDefenderInRange);
                        yield return new WaitForSeconds(AttackCooldown); 
                    }
                }
            }

            yield return null; 
        }
    }
}