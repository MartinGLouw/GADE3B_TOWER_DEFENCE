using System;
using UnityEngine;

public class NetProjectileNew : ProjectileNew
{
    public float disableDuration = 3f;
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

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender"))
        {
            DefenderNew defender = other.GetComponent<DefenderNew>();
            if (defender != null)
            {
                defender.DisableShooting(disableDuration); 
            }
        }
        base.OnTriggerEnter(other);
    }
}