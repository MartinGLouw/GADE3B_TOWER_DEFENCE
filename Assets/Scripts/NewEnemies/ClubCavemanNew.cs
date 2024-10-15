using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI; // Add this for UI elements

public class ClubCavemanNew : EnemyNew
{
    public GameObject clubProjectilePrefab;

    public float attackCooldown = 2f;
    public Slider healthSlider; // Add a health slider

    private void Start()
    {
        Health = 120f;
        Damage = 30f;
        AttackCooldown = 5f;

        // Initialize health slider
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
        GameObject projectile = Instantiate(clubProjectilePrefab, transform.position, Quaternion.identity);
        ClubProjectileNew clubProjectile = projectile.GetComponent<ClubProjectileNew>();

        if (clubProjectile != null)
        {
            clubProjectile.Initialize(target, 10f, Damage);
        }
    }

    // Add a method to update the health slider
    public void UpdateHealthSlider() 
    {
        healthSlider.value = Health;
    }
}