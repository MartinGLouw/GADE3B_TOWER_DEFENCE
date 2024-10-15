using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class DefenderNew : MonoBehaviour
{
    public float Health { get; protected set; }
    public float Damage { get; set; }
    public float AttackRange { get; protected set; } = 120f; 
    public int MeatCost { get; protected set; }
    public Slider healthSlider;

    private bool isTakingDamage = false;
    public bool canShoot = true; //Controls shooting
    protected float attackCooldown;

    protected virtual void Start()
    {
        healthSlider.maxValue = Health; 
        healthSlider.minValue = 0; 
        healthSlider.value = Health;
        DefendCoroutine();
    }

    public void TakeDamage(float damage)
    {
        if (!isTakingDamage)
        {
            Health -= damage;
            healthSlider.value = Health;

            if (Health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EP") && !isTakingDamage)
        {
            isTakingDamage = true; 
            TakeDamage(other.GetComponent<ProjectileNew>().Damage); 
            
            if (other.CompareTag("NetProjectile"))
            {
                NetProjectileNew netProjectile = other.GetComponent<NetProjectileNew>();
                if (netProjectile != null)
                {
                    DisableShooting(netProjectile.disableDuration);
                }
            }
            
            StartCoroutine(ResetDamageFlag());
        }
    }

    private IEnumerator ResetDamageFlag()
    {
        yield return new WaitForSeconds(0.5f); 
        isTakingDamage = false;
    }

    public void DisableShooting(float duration)
    {
        if (canShoot)
        {
            canShoot = false; 
            StartCoroutine(EnableShootingAfterDelay(duration));
        }
    }

    private IEnumerator EnableShootingAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        canShoot = true; //Re-enable shooting
    }

    protected abstract void LaunchProjectile(GameObject target); 
    public abstract IEnumerator DefendCoroutine();
}
