using UnityEngine;

public class ClubProjectileNew : ProjectileNew
{
    private float damage;

    [SerializeField] private BulletTrailScriptableObject bulletTrailConfig;
    private TrailRenderer trailRenderer;

    public override void Initialize(GameObject target, float speed, float damage)
    {
        base.Initialize(target, speed);
        this.damage = 30; 

        // Setup the Trail Renderer
        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        bulletTrailConfig.SetupTrail(trailRenderer);
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