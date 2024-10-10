using System.Collections;
using UnityEngine;

public class ShieldProjectile : MonoBehaviour
{
    public float lifetime = 5f; 
    public float returnSpeed = 10f; 
    private ShieldCaveman shieldCaveman; 
    public int damage = 20;

    private void Start()
    {
        shieldCaveman = FindObjectOfType<ShieldCaveman>();
        
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Defender"))
        {
            if (shieldCaveman != null)
            {
                shieldCaveman.HitDefender(other.gameObject);
            }
            
            StartCoroutine(ReturnToShieldCaveman());
        }
    }

    private IEnumerator ReturnToShieldCaveman()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = shieldCaveman.transform.position;

        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float startTime = Time.time;
        
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            float distCovered = (Time.time - startTime) * returnSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            
            transform.position = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
            yield return null; 
        }
        
        Destroy(gameObject);
    }
}
