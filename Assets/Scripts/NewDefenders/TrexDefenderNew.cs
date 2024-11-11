using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrexDefenderNew : DefenderNew
{
    public int attackBuff = 10;
    public float buffDuration = 5f;
    private bool isBuffed = false;
    public Vector2 Position { get; set; }
    public int meatCost = 50;

    public GameObject initialAppearance; 
    public GameObject firstUpgradeAppearance; 
    public GameObject secondUpgradeAppearance;

    public Slider healthSlider;

    private int upgradeLevel = 0; 
    private int upgradeCost = 100; 

    private void Start()
    {
        Health = 100;
        Damage = 0;
        AttackRange = 120f;
        MeatCost = meatCost;

        healthSlider.maxValue = Health;
        healthSlider.minValue = 0;
        healthSlider.value = Health;

        StartCoroutine(DefendCoroutine());
    }

    protected override void LaunchProjectile(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DefendCoroutine()
    {
        while (true)
        {
            GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");

            foreach (var defender in defenders)
            {
                DefenderNew defenderComponent = defender.GetComponent<DefenderNew>();
                if (defenderComponent != null && !isBuffed)
                {
                    defenderComponent.Damage += attackBuff;
                }
            }

            isBuffed = true;
            yield return new WaitForSeconds(buffDuration);

            foreach (var defender in defenders)
            {
                DefenderNew defenderComponent = defender.GetComponent<DefenderNew>();
                if (defenderComponent != null)
                {
                    defenderComponent.Damage -= attackBuff;
                }
            }

            isBuffed = false;
            yield return new WaitForSeconds(10f);
        }
    }

    public void Update()
    {
        UpdateHealthSlider();
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
            attackBuff += 5;
            Health += 50; // Increase health
            UpdateHealthSlider();

            //upgrade cost
            upgradeCost += 100;

            //Update the meat text
            FindObjectOfType<MeatManager>().UpdateMeatText();

            //Update upgrade level and appearance
            upgradeLevel++;
            UpdateAppearance();

            Debug.Log("T-Rex upgraded! New attack buff: " + attackBuff + ", New health: " + Health);
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
