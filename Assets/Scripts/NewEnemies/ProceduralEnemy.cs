using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProceduralEnemy : EnemyNew
{
    public GameObject proceduralProjectilePrefab;
    public Slider healthSlider;

    private void Start()
    {
        InitializeTraits();

        healthSlider.maxValue = Health;
        healthSlider.minValue = 0;
        healthSlider.value = Health;

        StartCoroutine(AttackCoroutine());
    }

    private void InitializeTraits()
    {
        Health = Random.Range(50, 200);
        Damage = Random.Range(10, 40);
        AttackCooldown = Random.Range(1f, 5f);

        Debug.Log($"Procedural Enemy Traits - Health: {Health}, Damage: {Damage}, Cooldown: {AttackCooldown}");
    }

    protected override IEnumerator AttackCoroutine()
    {
        Debug.Log($"{this.name} started Attacking.");
        while (true)
        {
            GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
            if (defenders.Length > 0)
            {
                GameObject[] defendersInRange = defenders
                    .Where(defender => Vector3.Distance(transform.position, defender.transform.position) <= AttackRange)
                    .ToArray();

                if (defendersInRange.Length > 0)
                {
                    GameObject closestDefenderInRange = defendersInRange
                        .OrderBy(defender => Vector3.Distance(transform.position, defender.transform.position))
                        .FirstOrDefault();

                    if (closestDefenderInRange != null && canShoot)
                    {
                        Debug.Log($"{this.name} found defender {closestDefenderInRange.name} in range.");
                        LaunchProjectile(closestDefenderInRange);
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
        //Instantiate the projectile prefab
        GameObject projectileInstance = Instantiate(proceduralProjectilePrefab, transform.position, Quaternion.identity);

        //Initialize the projectile
        ProceduralProjectile projectile = projectileInstance.GetComponent<ProceduralProjectile>();
        if (projectile != null)
        {
            projectile.Initialize(target, 10f, Damage);
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        if (Health > healthSlider.maxValue) Health = healthSlider.maxValue; //Ensure health doesn't exceed max
        UpdateHealthSlider();
        Debug.Log($"Procedural Enemy healed for {amount}, new health: {Health}");
    }

    protected override void Die()
    {
        //Launch a projectile at every defender on the map upon death
        GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
        foreach (GameObject defender in defenders)
        {
            LaunchProjectile(defender);
        }

        base.Die();
    }

    public void UpdateHealthSlider()
    {
        healthSlider.value = Health;
    }
}
