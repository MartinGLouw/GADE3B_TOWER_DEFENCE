using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    
    public static Tower tower;
    public EnemyFactoryNew enemyFactoryNew; 
    private List<DefenderNew> defenders = new List<DefenderNew>();

   
    [System.Serializable]
    public struct DefenderSpawnInfo
    {
        public DefenderFactoryNew.DefenderType type;
        public Vector2 position;
    }

    public DefenderSpawnInfo[] defenderTypes; 

    void Start()
    {
       
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

 

    
}