using System.Collections;
using UnityEngine;

public class NetProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    public float returnSpeed = 10f;
    public float disableDuration = 2f;
    private NetCaveman netCaveman;

    private void Start()
    {
        netCaveman = FindObjectOfType<NetCaveman>();
        
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Defender"))
        {
            if (netCaveman != null)
            {
                netCaveman.HitDefender(other.gameObject, disableDuration);
            }
            
            StartCoroutine(ReturnToNetCaveman());
        }
    }

    private IEnumerator ReturnToNetCaveman()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = netCaveman.transform.position;

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
