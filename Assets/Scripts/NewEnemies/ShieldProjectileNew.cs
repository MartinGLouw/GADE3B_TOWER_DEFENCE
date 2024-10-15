using UnityEngine;
using System.Collections;

public class ShieldProjectileNew : ProjectileNew
{
    private float damage;
    private bool hasHitFirstTime = false;
    private bool returningToShooter = false;
    private bool hasHitOnReturn = false;
    private Transform shooterTransform;
    private Vector3 flyPastPoint;

    public void Initialize(GameObject target, float speed, float damage, Transform shooter)
    {
        base.Initialize(target, speed);
        this.damage = 10;
        this.shooterTransform = shooter;
        flyPastPoint = target.transform.position + (target.transform.position - shooter.position).normalized * 2f;
    }

    protected void Update()
    {
        if (!returningToShooter)
        {
            transform.position = Vector3.MoveTowards(transform.position, flyPastPoint, speed * Time.deltaTime);


            if (Vector3.Distance(transform.position, flyPastPoint) < 0.1f && !hasHitFirstTime)
            {
                DealDamage();
                returningToShooter = true;
            }
        }
        else
        {
            transform.position =
                Vector3.MoveTowards(transform.position, shooterTransform.position, speed * Time.deltaTime);


            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f && !hasHitOnReturn)
            {
                DealDamage();
                hasHitOnReturn = true;
            }


            if (Vector3.Distance(transform.position, shooterTransform.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    protected override void DealDamage()
    {
        DefenderNew defender = target.GetComponent<DefenderNew>();
        if (defender != null)
        {
            defender.TakeDamage(damage);
        }
        else
        {
            Debug.LogError("Target does not have a DefenderNew component!");
        }
    }
}