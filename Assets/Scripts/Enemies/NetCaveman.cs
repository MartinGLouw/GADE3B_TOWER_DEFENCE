using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetCaveman : Enemy
{
    public NetCaveman()
    {
        Health = 200;
        Damage = 0;
        AttRange = 120; 
    }

    public Slider healthSliderNet;
    public DefenProjectile DefenProjectile;

    private void Start()
    {
        if (healthSliderNet != null)
        {
            healthSliderNet.maxValue = Health; 
            healthSliderNet.minValue = 0; 
            healthSliderNet.value = Health; 
        }
    }

    private void Update()
    {
        if (healthSliderNet != null)
        {
            healthSliderNet.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
            healthSliderNet.value = Health; 
        }

        if (Health <= 0)
        {
            // You can add additional behavior here when the NetCaveman dies
        }
    }

    public override void Attack()
    {
        Debug.Log("NetCaveman throws a net!");
    }

    public void HitDefender(GameObject defender, float disableDuration)
    {
        Defender defenderScript = defender.GetComponent<Defender>();
        if (defenderScript != null)
        {
            StartCoroutine(DisableDefender(defenderScript, disableDuration));
        }
        else
        {
            Debug.LogError("Hit object does not have a Defender component!");
        }
    }

    private IEnumerator DisableDefender(Defender defender, float duration)
    {
        defender.enabled = false; // Disables the defender
        yield return new WaitForSeconds(duration);
        defender.enabled = true; // Enables the defender
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DP")) // Replace "DP" with your damage projectile tag if needed
        {
            if (Health > 0)
            {
                Health -= DefenProjectile.damage; // Assuming EnemyProjectile has a damage property
                healthSliderNet.value = Health;
            }

            if (Health <= 0)
            {
                Destroy(gameObject); // Handle destruction logic
            }
        }
    }
}
