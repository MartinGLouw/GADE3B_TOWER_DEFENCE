using System.Collections;
using UnityEngine;

public class ProceduralProjectile : ProjectileNew
{
    private float damage;
    private ProceduralEnemy ownerEnemy;

    public override void Initialize(GameObject target, float speed, float damage)
    {
        base.Initialize(target, speed, damage);
        this.damage = damage;
        ownerEnemy = GetComponentInParent<ProceduralEnemy>();
    }

    protected override void DealDamage()
    {
        DefenderNew defender = target.GetComponent<DefenderNew>();
        if (defender != null)
        {
            defender.TakeDamage(damage);

            // Heal the owner enemy for 10% of the damage dealt
            float healAmount = damage * 0.1f;
            ownerEnemy.Heal(healAmount);
        }
        else
        {
            Debug.LogError("Target does not have a DefenderNew component!");
        }
    }
}