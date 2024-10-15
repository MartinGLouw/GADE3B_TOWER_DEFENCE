using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; 

public class NetCavemanNew : EnemyNew
{
    public GameObject netProjectilePrefab;
    public Slider healthSlider;

    private void Start()
    {
        Health = 120f;
        Damage = 0f;
        AttackCooldown = 15f;
        
        healthSlider.maxValue = Health;
        healthSlider.minValue = 0;
        healthSlider.value = Health;

        StartCoroutine(AttackCoroutine());
    }

    protected override IEnumerator AttackCoroutine()
    {
        Debug.Log($"{this.name} started Attacking.");
        while (true)
        {
            GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
            Debug.Log($"defender detected: {defenders.Length}");

            if (defenders.Length > 0)
            {
                GameObject[] defendersInRange = defenders
                    .Where(defender => Vector3.Distance(transform.position, defender.transform.position) <= AttackRange)
                    .ToArray();
                Debug.Log($"defender in range: {defendersInRange.Length}");

                if (defendersInRange.Length > 0)
                {
                    GameObject closesDefenderInRange = defendersInRange
                        .OrderBy(defender => Vector3.Distance(transform.position, defender.transform.position))
                        .FirstOrDefault();

                    if (closesDefenderInRange != null && canShoot)
                    {
                        Debug.Log($"{this.name} found defender {closesDefenderInRange.name} in range.");
                        LaunchProjectile(closesDefenderInRange);
                        yield return new WaitForSeconds(AttackCooldown); 
                    }
                }
            }

            yield return null; 
        }
    }
    public void Update()
    {
        UpdateHealthSlider();
    }

    protected override void LaunchProjectile(GameObject target)
    {
        GameObject projectileInstance = Instantiate(netProjectilePrefab, transform.position, Quaternion.identity);
        NetProjectileNew netProjectile = projectileInstance.GetComponent<NetProjectileNew>();

        if (netProjectile != null)
        {
            netProjectile.Initialize(target, 10f, 3f); 
        }
    }
    public void UpdateHealthSlider() 
    {
        healthSlider.value = Health;
    }
}