using UnityEngine;

public class StegoProjectileNew : DefenderProjectileNew
{
    private void Start()
    {
        speed = 15f;
        damage = 10; 
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