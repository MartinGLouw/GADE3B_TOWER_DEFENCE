using UnityEngine;

public class ClubCavemanNew : EnemyNew
{
    public GameObject clubProjectilePrefab;

    private void Start()
    {
        Health = 120f;
        Damage = 30f;
        AttackCooldown = 2f;
    }

    public override void Attack()
    {
        GameObject closestDefender = FindClosestDefender();
        if (closestDefender != null)
        {
            LaunchProjectile(closestDefender);
        }
    }

    private void LaunchProjectile(GameObject target)
    {
        GameObject projectileInstance = Instantiate(clubProjectilePrefab, transform.position, Quaternion.identity);
        ClubProjectileNew clubProjectile = projectileInstance.GetComponent<ClubProjectileNew>();

        if (clubProjectile != null)
        {
            clubProjectile.Initialize(target, 10f, Damage);
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