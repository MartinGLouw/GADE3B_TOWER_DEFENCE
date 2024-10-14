using System;
using UnityEngine;

public class NetProjectileNew : ProjectileNew
{
    public float disableDuration = 5f;
    private float damage;
    

    public void Start()
    {
        damage = 0;
    }

    public override void Initialize(GameObject target, float speed, float disableDuration)
    {
        base.Initialize(target, speed);
        this.disableDuration = disableDuration;
    }
    

    protected override void DealDamage()
    {
        DefenderNew defender = target.GetComponent<DefenderNew>();
        if (defender != null)
        {
            defender.DisableShooting(disableDuration); 
        }
        else
        {
            Debug.LogError("Target does not have an EnemyNew component!");
        }
    }
}