using System.Collections;
using UnityEngine;

public abstract class EnemyNew : MonoBehaviour
{
    public float Health { get; set; }
    public float Damage { get; protected set; }
    public float AttackRange { get; protected set; } = 120f;
    public float AttackCooldown { get; protected set; }

    protected float nextAttackTime;
    public bool canShoot = true;
    public virtual void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    protected virtual IEnumerator AttackCoroutine()
    {
        yield return null;
    }

    private void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + AttackCooldown;
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
    protected abstract void LaunchProjectile(GameObject target); 
}