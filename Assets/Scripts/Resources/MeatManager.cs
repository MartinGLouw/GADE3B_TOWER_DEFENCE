using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class MeatManager : MonoBehaviour
{
    public static int meat = 20;
    public int meatPerSecond = 1;
    public TextMeshProUGUI meatText;
    

    void Start()
    {
        InvokeRepeating("GenerateMeat", 1.0f, 1.0f); //Generate meat every second
        UpdateMeatText();
    }

    void GenerateMeat()
    {
        meat += meatPerSecond;
        UpdateMeatText();
        Debug.Log($"Meat: {meat}");
    }

    public void AddMeat(int amount)
    {
        meat += amount;
        UpdateMeatText();
        Debug.Log($"Meat: {meat}");
    }
    public void UpdateMeatText()
    {
        meatText.text = $"Meat: {meat}";
    }
}
