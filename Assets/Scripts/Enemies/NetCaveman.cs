using System;
using System.Collections;
using UnityEngine;

public class NetCaveman : Enemy
{
    public NetCaveman()
    {
        Health = 150;
        Damage = 0; //Net disables, doesn't deal damage
        AttRange = 100;
    }

    public GameObject netPrefab;
    public float projectileSpeed = 12f;
    public float attackCooldown = 15f;
    public float disableDuration = 2f;

    private bool canThrowNet = true;

    private void Start()
    {
        StartCoroutine(NetThrowCoroutine());
    }

    public override void Attack()
    {
        throw new NotImplementedException();
    }

    private IEnumerator NetThrowCoroutine()
    {
        while (true)
        {
            if (canThrowNet)
            {
                GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");

                if (defenders.Length > 0)
                {
                    GameObject[] defendersInRange = Array.FindAll(defenders, defender =>
                        Vector3.Distance(transform.position, defender.transform.position) <= AttRange);

                    if (defendersInRange.Length > 0)
                    {
                        GameObject closestDefender = defendersInRange[0];

                        //Throw the net
                        ThrowNet(closestDefender);
                        canThrowNet = false;
                        yield return new WaitForSeconds(attackCooldown); //Wait 15 seconds before throwing again
                        canThrowNet = true;
                    }
                }
            }
            yield return null;
        }
    }

    private void ThrowNet(GameObject target)
    {
        GameObject netProjectile = Instantiate(netPrefab, transform.position, Quaternion.identity);
        Rigidbody netRigidbody = netProjectile.GetComponent<Rigidbody>();

        if (netRigidbody != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            netRigidbody.velocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogError("Net prefab is missing a Rigidbody component!");
        }

        StartCoroutine(DisableDefender(target));
    }

    private IEnumerator DisableDefender(GameObject defender)
    {
        Defender defenderScript = defender.GetComponent<Defender>();

        if (defenderScript != null)
        {
            defenderScript.enabled = false; //Disable defender
            yield return new WaitForSeconds(disableDuration); //Wait for 2 seconds
            defenderScript.enabled = true; //Re-enable defender
        }
        else
        {
            Debug.LogError("Target does not have a Defender component!");
        }
    }
}
