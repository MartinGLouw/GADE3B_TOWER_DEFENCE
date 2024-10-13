using UnityEngine;

public class ClubProjectileNew : ProjectileNew
{
    private float damage;

    public override void Initialize(GameObject target, float speed, float damage)
    {
        base.Initialize(target, speed);
        this.damage = 30; 
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
            Debug.LogError("Target does not have an EnemyNew component!");
        }
    }
    

   
}