using UnityEngine;

public class RaptorProjectileNew : DefenderProjectileNew
{
    private void Start()
    {
        speed = 15f;
        damage = 30; 
    }

    protected override void DealDamage()
    {
        
        EnemyNew enemy = target.GetComponent<EnemyNew>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        else
        {
            Debug.LogError("Target does not have an EnemyNew component!");
        }
    }
}