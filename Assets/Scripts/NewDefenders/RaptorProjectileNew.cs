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
            
            // Instantiate and play the particle effect
            GameObject particleEffect = Instantiate(damageParticleEffectPrefab, transform.position, Quaternion.identity);
            ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                Destroy(particleEffect, ps.main.duration); // Destroy particle effect after its duration
            }
            else
            {
                Debug.LogError("Particle system component not found on the instantiated prefab!");
            }
        }
        else
        {
            Debug.LogError("Target does not have an EnemyNew component!");
        }
    }
}