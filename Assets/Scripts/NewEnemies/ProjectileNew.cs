using System.Collections;
using UnityEngine;

public abstract class ProjectileNew : MonoBehaviour
{
    protected float Speed = 10f;
    protected GameObject target;
    public int Damage { get; set; }

   
    public virtual void Initialize(GameObject target, float speed, float damage = 0f)
    {
        this.target = target;
        this.Speed = speed;
        Damage = 10;
        StartCoroutine(MoveToTarget());
    }
    
    protected virtual IEnumerator MoveToTarget()
    {
        while (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * Speed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject); 
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject); 
    }
}