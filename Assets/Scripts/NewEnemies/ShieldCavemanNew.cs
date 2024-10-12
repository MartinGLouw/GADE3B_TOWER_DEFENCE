using UnityEngine;
using System.Collections;

public class ShieldCavemanNew : EnemyNew
{
    public GameObject shieldProjectilePrefab;

    private void Start()
    {
        Health = 120f;
        Damage = 20f; 
        AttackCooldown = 3f;
    }

    public override void Attack()
    {
        GameObject closestDefender = FindClosestDefender();
        if (closestDefender != null)
        {
            LaunchShieldProjectile(closestDefender);
        }
    }

    private void LaunchShieldProjectile(GameObject target)
    {
        GameObject projectileInstance = Instantiate(shieldProjectilePrefab, transform.position, Quaternion.identity);
        ShieldProjectileNew shieldProjectile = projectileInstance.GetComponent<ShieldProjectileNew>();

        if (shieldProjectile != null)
        {
            shieldProjectile.Initialize(target, 10f, Damage); 
        }
    }

    private GameObject FindClosestDefender()
    {
        GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
        GameObject closestDefender = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject defender in defenders)
        {
            float distance = Vector3.Distance(transform.position, defender.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDefender = defender;
            }
        }

        return closestDefender;
    }
}