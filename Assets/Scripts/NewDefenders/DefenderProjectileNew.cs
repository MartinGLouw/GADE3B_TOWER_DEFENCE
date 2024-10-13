using System.Collections;
using UnityEngine;

public abstract class DefenderProjectileNew : MonoBehaviour
{
    protected float speed;
    protected int damage; 
    protected GameObject target;

    public void Initialize(GameObject target, float projectileSpeed, int projectileDamage)
    {
        Debug.Log($"Projectile initialized, targeting {target.name}");
        this.target = target;
        this.speed = projectileSpeed;
        this.damage = projectileDamage;
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            
            if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            {
                DealDamage();
                Destroy(gameObject);
            }

            yield return null;
        }
        Destroy(gameObject); 
    }

    protected abstract void DealDamage(); 
}