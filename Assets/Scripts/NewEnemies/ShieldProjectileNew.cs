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

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defender") && !hasHitFirstTime)
        {
            hasHitFirstTime = true;
            DefenderNew defender = other.GetComponent<DefenderNew>();
            if (defender != null)
            {
                defender.TakeDamage(damage); 
                StartCoroutine(SecondHit(defender)); 
            }
        }
        base.OnTriggerEnter(other);  
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