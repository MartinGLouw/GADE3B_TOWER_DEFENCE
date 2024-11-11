using System.Collections;
using UnityEngine;

public class NetProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public float disableDuration = 2f;
    private NetCaveman netCaveman;
    public int damage = 0;

    [SerializeField] private BulletTrailScriptableObject bulletTrailConfig;
    private TrailRenderer trailRenderer;

    private void Start()
    {
        // Setup the Trail Renderer
        trailRenderer = gameObject.AddComponent<TrailRenderer>();
        bulletTrailConfig.SetupTrail(trailRenderer);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Defender"))
        {
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            if (netCaveman != null)
            {
                netCaveman.HitDefender(other.gameObject, disableDuration);
            }
        }
    }
}