using UnityEngine;

public class NetCavemanNew : EnemyNew
{
    public GameObject netProjectilePrefab;

    private void Start()
    {
        Health = 120f;
        Damage = 0f;
        AttackCooldown = 15f;
    }

    public override void Attack()
    {
        GameObject closestDefender = FindClosestDefender();
        if (closestDefender != null)
        {
            LaunchNetProjectile(closestDefender);
        }
    }

    private void LaunchNetProjectile(GameObject target)
    {
        GameObject projectileInstance = Instantiate(netProjectilePrefab, transform.position, Quaternion.identity);
        NetProjectileNew netProjectile = projectileInstance.GetComponent<NetProjectileNew>();

        if (netProjectile != null)
        {
            netProjectile.Initialize(target, 10f, 3f); 
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