using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatManager : MonoBehaviour
{
    public int meat = 0;
    public int meatPerSecond = 1;

    void Start()
    {
        InvokeRepeating("GenerateMeat", 1.0f, 1.0f); // Generate gold every second
    }

    void GenerateMeat()
    {
        meat += meatPerSecond;
        Debug.Log($"Meat: {meat}");
    }

    public void addMeat(int amount)
    {
        meat += amount;
        Debug.Log($"Meat: {meat}");
    }
}
