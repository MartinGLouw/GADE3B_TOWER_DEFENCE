using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f; // Adjust the lifetime as needed
    public Defender defender;
    private void Start()
    {
        // Start the lifetime timer
        Destroy(gameObject, lifetime);
    }

    public  void OnTriggerEnter(Collider other)
    {
        // Check if the collision is with an enemy
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EnemyHit");
            
            // Optionally, apply damage to the enemy here
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
               
            }

            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}