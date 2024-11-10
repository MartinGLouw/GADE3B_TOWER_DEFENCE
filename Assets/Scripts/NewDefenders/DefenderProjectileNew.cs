using System.Collections;
using UnityEngine;

public abstract class DefenderProjectileNew : MonoBehaviour
{
    protected float speed;
    protected int damage; 
    protected GameObject target;

    [SerializeField] private BulletTrailScriptableObject bulletTrailConfig;
    [SerializeField] private GameObject damageParticleEffectPrefab; // Use GameObject type for the prefab
    private TrailRenderer trailRenderer;

    public void Initialize(GameObject target, float projectileSpeed, int projectileDamage)
    {
        Debug.Log($"Projectile initialized, targeting {target.name}");
        this.target = target;
        this.speed = projectileSpeed;
        this.damage = projectileDamage;

        // Setup the Trail Renderer
        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        bulletTrailConfig.SetupTrail(trailRenderer);

        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                DealDamage();
                Destroy(gameObject);
            }

            yield return null;
        }
        Destroy(gameObject); 
    }

    protected virtual void DealDamage()
    {
        // Instantiate and play the particle effect at the point of impact
        if (damageParticleEffectPrefab != null)
        {
            Instantiate(damageParticleEffectPrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();
        }
    }
}