using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public int attackDamage = 50;
    public float attackCooldown = 5f;
    private float lastAttackTime;
    private SphereCollider sphereCollider;
    public int AttRange = 100;
    private List<EnemyNew> enemiesInRange = new List<EnemyNew>();
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int Health = 600;
    public int Damage = 30;
    public bool dead = false;
    public GameObject endScreenCanvas;
    public TextMeshProUGUI TowerHealthText;
    public TextMeshProUGUI UpgradeText;
    public Button upgradeButton;

    public GameObject initialAppearance; // Reference to the initial appearance of the tower
    public GameObject firstUpgradeAppearance; // Reference to the appearance after the first upgrade
    public GameObject secondUpgradeAppearance; // Reference to the appearance after the second upgrade

    private int upgradeLevel = 0; // Track the number of upgrades
    private int upgradeCost = 100; // Initial upgrade cost

    void Start()
    {
        Health = 600;
        Damage = 30;
        AttRange = 150;
        dead = false;
        endScreenCanvas.SetActive(false);
        UpdateTowerHealthText();
        StartCoroutine(TowerDefense());

        upgradeButton.onClick.AddListener(UpgradeTower); // Link the button to the upgrade method
    }

    private IEnumerator TowerDefense()
    {
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length > 0)
            {
                GameObject[] enemiesInRange = enemies
                    .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= AttRange)
                    .ToArray();

                if (enemiesInRange.Length > 0)
                {
                    GameObject closestEnemyInRange = enemiesInRange
                        .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                        .FirstOrDefault();

                    if (closestEnemyInRange != null)
                    {
                        Debug.Log("Tower attacks!");
                        LaunchProjectile(closestEnemyInRange);
                        yield return new WaitForSeconds(attackCooldown);
                    }
                }
            }

            yield return null;
        }
    }
    
    private void LaunchProjectile(GameObject target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

        if (projectileRigidbody != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            projectileRigidbody.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Projectile prefab is missing a Rigidbody component!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyNew enemy = other.GetComponent<EnemyNew>();
        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
        if (other.gameObject.CompareTag("EP"))
        {
            if (Health > 0)
            {
                Health -= Damage;
                UpdateTowerHealthText();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        EnemyNew enemy = other.GetComponent<EnemyNew>();
        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    void Update()
    {
        UpdateTowerHealthText();
        CheckDeath();
        if (enemiesInRange.Count > 0 && Time.time > lastAttackTime + attackCooldown)
        {
            Attack(enemiesInRange[0]);
            lastAttackTime = Time.time;
        }
    }

    void Attack(EnemyNew enemy)
    {
        Debug.Log("Tower attacks!");
        enemy.Health -= attackDamage;
    }

    void CheckDeath()
    {
        if (Health == 0)
        {
            Debug.Log("Tower is dead.");
            dead = true;
            endScreenCanvas.SetActive(true);
        }
    }

    public void UpdateTowerHealthText()
    {
        TowerHealthText.SetText("Tower Health: " + Health.ToString());
    }

    public void UpgradeTower()
    {
        if (MeatManager.meat >= upgradeCost)
        {
            // Deduct the meat cost
            MeatManager.meat -= upgradeCost;

            // Increase stats
            attackDamage += 10;
            attackCooldown -= 0.5f;
            if (attackCooldown < 1f) attackCooldown = 1f; // Prevent cooldown from becoming too low
            AttRange += 20;
            Health += 100;
            UpdateTowerHealthText();

            // Increment the upgrade cost
            upgradeCost += 100;

            // Update the meat text
            FindObjectOfType<MeatManager>().UpdateMeatText();

            // Update upgrade status
            upgradeLevel++;
            UpdateAppearance();
            UpgradeText.SetText("Tower Upgraded! Attack Damage: " + attackDamage + ", Cooldown: " + attackCooldown + "s, Range: " + AttRange + ", Health: " + Health);
            Debug.Log("Tower upgraded!");
        }
        else
        {
            UpgradeText.SetText("Not enough meat to upgrade!");
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
