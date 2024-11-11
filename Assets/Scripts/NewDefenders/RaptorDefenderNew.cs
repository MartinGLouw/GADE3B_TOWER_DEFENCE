using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RaptorDefenderNew : DefenderNew
{
    public GameObject projectilePrefab; 
    public float projectileSpeed = 15f; 
    public float attackCooldown = 2f; 
    public int meatCost = 20;
    public int damage = 30;
    public int Health;

    public GameObject initialAppearance; 
    public GameObject firstUpgradeAppearance; 
    public GameObject secondUpgradeAppearance; 

    public Slider healthSlider;

    private int upgradeLevel = 0; 
    private int upgradeCost = 100; 

    private void Start()
    {
        Health = 100;
        Damage = damage; 
        MeatCost = meatCost; 
        
        healthSlider.maxValue = Health;
        healthSlider.minValue = 0;
        healthSlider.value = Health;
        
        StartCoroutine(DefendCoroutine());
    }

    public override IEnumerator DefendCoroutine()
    {
        Debug.Log($"{this.name} started defending.");
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Debug.Log($"Enemies detected: {enemies.Length}");

            if (enemies.Length > 0)
            {
                GameObject[] enemiesInRange = enemies
                    .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= AttackRange)
                    .ToArray();
                Debug.Log($"Enemies in range: {enemiesInRange.Length}");

                if (enemiesInRange.Length > 0)
                {
                    GameObject closestEnemyInRange = enemiesInRange
                        .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                        .FirstOrDefault();

                    if (closestEnemyInRange != null && canShoot)
                    {
                        Debug.Log($"{this.name} found enemy {closestEnemyInRange.name} in range.");
                        LaunchProjectile(closestEnemyInRange);
                        yield return new WaitForSeconds(attackCooldown); 
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
        Debug.Log($"{this.name} is launching a projectile at {target.name}.");
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        RaptorProjectileNew raptorProjectile = projectile.GetComponent<RaptorProjectileNew>();

        if (raptorProjectile != null)
        {
            raptorProjectile.Initialize(target, projectileSpeed, damage); 
        }
    }

    public void UpdateHealthSlider() 
    {
        healthSlider.value = Health;
    }

    public override void Upgrade()
    {
        if (MeatManager.meat >= upgradeCost)
        {
            //Deduct the meat cost
            MeatManager.meat -= upgradeCost;

            //Increase stats
            Damage += 10;
            Health += 50;
            attackCooldown -= 0.5f;
            if (attackCooldown < 1f) attackCooldown = 1f; 
            UpdateHealthSlider();

            //Upgrade cost
            upgradeCost += 100;

            //Update the meat text
            FindObjectOfType<MeatManager>().UpdateMeatText();

            //Update upgrade level and appearance
            upgradeLevel++;
            UpdateAppearance();
            
            Debug.Log("Raptor upgraded! New damage: " + Damage + ", New health: " + Health);
        }
        else
        {
            Debug.Log("Not enough meat to upgrade!");
        }
    }

    private void UpdateAppearance()
    {
        if (upgradeLevel == 1)
        {
            initialAppearance.SetActive(false);
            firstUpgradeAppearance.SetActive(true);
        }
        else if (upgradeLevel == 2)
        {
            firstUpgradeAppearance.SetActive(false);
            secondUpgradeAppearance.SetActive(true);
        }
    }
}
