using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 5f; 

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with an enemy
        if (other.gameObject.CompareTag("Defender"))
        {
            // Optionally, apply damage to the enemy here
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
                // ... apply damage to the enemy
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}