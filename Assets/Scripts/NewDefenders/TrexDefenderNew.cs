using System.Collections;
using UnityEngine;

public class TrexDefenderNew : DefenderNew
{
    public int attackBuff = 10; 
    public float buffDuration = 5f; 
    private bool isBuffed = false;
    public Vector2 Position { get; set; }
    public int meatCost = 50;
    

    private void Start()
    {
        Health = 100; 
        Damage = 0; 
        AttackRange = 120f;
        MeatCost = meatCost;
        StartCoroutine(BuffDefendersCoroutine());
    }

    protected override void LaunchProjectile(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator DefendCoroutine()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator BuffDefendersCoroutine()
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

  
}