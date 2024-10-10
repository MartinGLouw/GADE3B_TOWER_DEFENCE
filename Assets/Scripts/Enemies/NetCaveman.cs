using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NetCaveman : Enemy
{
    
    public NetCaveman()
    {
        Health = 200;
        Damage = 0;
        AttRange = 25; 
    }
    public Slider healthSlider;
    private void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = Health; 
            healthSlider.minValue = 0; 
            healthSlider.value = Health; 
        }
    }
    private void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up);
            healthSlider.value = Health;
        }
        
        if (Health <= 0)
        {
           
        }
    }



    public override void Attack()
    {
       
        Debug.Log("NetCaveman throws a net!");
    }

    public void HitDefender(GameObject defender, float disableDuration)
    {
        Defender defenderScript = defender.GetComponent<Defender>();
        if (defenderScript != null)
        {
            StartCoroutine(DisableDefender(defenderScript, disableDuration));
        }
        else
        {
            Debug.LogError("Hit object does not have a Defender component!");
        }
    }

    private IEnumerator DisableDefender(Defender defender, float duration)
    {
        defender.enabled = false; //Disables the defender
        yield return new WaitForSeconds(duration);
        defender.enabled = true; //enables the defender
    }
}