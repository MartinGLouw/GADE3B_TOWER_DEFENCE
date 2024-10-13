using UnityEngine;
using System.Collections;

public class ShieldProjectileNew : ProjectileNew
{
    private float damage;
    private bool hasHitFirstTime = false;

    public override void Initialize(GameObject target, float speed, float damage)
    {
        base.Initialize(target, speed);
        this.damage = 20;
    }

    protected override void DealDamage()
    {
        if (!hasHitFirstTime)
        {
            hasHitFirstTime = true;
            DefenderNew defender = target.GetComponent<DefenderNew>();
            if (defender != null)
            {
                defender.TakeDamage(damage);
                StartCoroutine(SecondHit(defender));
            }
            else
            {
                Debug.LogError("Target does not have an EnemyNew component!");
            }
        }
    }

    private IEnumerator SecondHit(DefenderNew defender)
    {
        yield return new WaitForSeconds(1f);
        if (defender != null)
        {
            defender.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}