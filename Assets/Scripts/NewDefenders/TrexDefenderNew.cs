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
    public Slider healthSlider; // Add a health slider
    
    

    private void Start()
    {
        Health = 100; 
        Damage = 0; 
        AttackRange = 120f;
        MeatCost = meatCost;

        // Initialize health slider
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

    // Add a method to update the health slider
    public void UpdateHealthSlider() 
    {
        healthSlider.value = Health;
    }
    
}