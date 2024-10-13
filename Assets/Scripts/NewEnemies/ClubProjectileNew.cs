using UnityEngine;

public class ClubProjectileNew : ProjectileNew
{
    private float damage;

    public override void Initialize(GameObject target, float speed, float damage)
    {
        base.Initialize(target, speed);
        this.damage = 30; 
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender")) 
        {
            DefenderNew defender = other.GetComponent<DefenderNew>();
            if (defender != null)
            {
                defender.TakeDamage(damage); 
            }
        }
        base.OnTriggerEnter(other); 
    }
}