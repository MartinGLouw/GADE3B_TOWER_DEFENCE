using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MeatManager : MonoBehaviour
{
    public int meat = 0;
    public int meatPerSecond = 1;
    public TextMeshProUGUI meatText;
    

    void Start()
    {
        InvokeRepeating("GenerateMeat", 1.0f, 1.0f); // Generate gold every second
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
    void UpdateMeatText()
    {
        meatText.text = $"Meat: {meat}";
    }
}
