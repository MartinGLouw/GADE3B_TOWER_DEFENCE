using UnityEngine;

public class DefenProjectile : MonoBehaviour
{
    public float lifetime = 5f; 
    public int damage = 30;

    [SerializeField] private BulletTrailScriptableObject bulletTrailConfig;
    private TrailRenderer trailRenderer;

    private void Start()
    {
        // Setup the Trail Renderer
        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        bulletTrailConfig.SetupTrail(trailRenderer);

        Destroy(gameObject, lifetime);
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if collided
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EnemyHit");
            
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                // Implement the interaction with the enemy
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}