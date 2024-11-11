using UnityEngine;

public class DefenProjectile : DefenderProjectileNew
{
    public float lifetime = 5f; 

    [SerializeField] private BulletTrailScriptableObject bulletTrailConfig;
    private TrailRenderer trailRenderer;

    private void Start()
    {
        // Setup the Trail Renderer
        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        bulletTrailConfig.SetupTrail(trailRenderer);

        Destroy(gameObject, lifetime);
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
        Destroy(gameObject);
    }
}