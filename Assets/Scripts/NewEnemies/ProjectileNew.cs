using System.Collections;
using UnityEngine;

public abstract class ProjectileNew : MonoBehaviour
{
    
    protected GameObject target;
    public int Damage { get; set; }
    protected float speed;
    public bool canShoot = true;
   
    public virtual void Initialize(GameObject target, float projectileSpeed, float damage = 0f)
    {
        this.target = target;
        this.speed = projectileSpeed;
        Damage = 10;
        StartCoroutine(MoveToTarget());
    }
    
    protected IEnumerator MoveToTarget()
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