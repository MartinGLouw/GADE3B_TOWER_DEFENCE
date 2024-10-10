using UnityEngine;

public class DefenProjectile : MonoBehaviour
{
    public float lifetime = 5f; 
    public int damage = 30;
   
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public  void OnTriggerEnter(Collider other)
    {
        //Check if collided
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("EnemyHit");
            
            IEnemy enemy = other.gameObject.GetComponent<IEnemy>();
            if (enemy != null)
            {
               
            }

            //Destroy the projectile
            Destroy(gameObject);
        }
    }
}