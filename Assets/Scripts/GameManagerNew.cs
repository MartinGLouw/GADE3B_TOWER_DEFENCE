using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManagerNew : MonoBehaviour
{
    public MeatManager meatManager;
    public TextMeshProUGUI meatText;
    public TextMeshProUGUI towerHealthText;
    public Tower tower;
    public EnemyFactoryNew enemyFactoryNew; 
    private List<DefenderNew> defenders = new List<DefenderNew>();

   
    [System.Serializable]
    public struct DefenderSpawnInfo
    {
        public DefenderFactoryNew.DefenderType type;
        public Vector3 position;
    }

    public DefenderSpawnInfo[] defenderTypes; 

    void Start()
    {
        UpdateTowerHealthText();
        SpawnDefenders(); 
    }

    private void SpawnDefenders()
    {
        foreach (var defenderInfo in defenderTypes)
        {
            DefenderNew defender = DefenderFactoryNew.CreateDefender<DefenderNew>(defenderInfo.type, defenderInfo.position);
            if (defender != null)
            {
                defenders.Add(defender);
                StartCoroutine(defender.DefendCoroutine());
            }
            else
            {
                Debug.LogError($"Defender of type {defenderInfo.type} could not be created.");
            }
        }
    }

    void Update()
    {
        towerHealthText.text = $"Tower Health: {tower.Health}";
    }

    public void UpdateTowerHealthText()
    {
        towerHealthText.text = $"Tower Health: {tower.Health}";
    }
}