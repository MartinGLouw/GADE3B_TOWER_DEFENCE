using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public MeatManager meatManager;
    public TextMeshProUGUI meatText;
    public TextMeshProUGUI towerHealthText;
    public Tower tower;
    void Start()
    {
        
        UpdateTowerHealthText();
        EnemyFactory enemyFactory = new EnemyFactory();
        List<IDefender> defenders = new List<IDefender>
        {
            new RaptorDefender() { Position = new Vector2(1, 1) },
            new TrexDefender() { Position = new Vector2(2, 2) },
            new StegoDefender() { Position = new Vector2(3, 3) }
        };
        Vector2 towerPosition = new Vector2(0, 0);

        IEnemy clubCaveman = enemyFactory.CreateEnemy("ClubCaveman");
        clubCaveman.Position = new Vector2(1, 0);
        clubCaveman.AttackClosest(defenders, towerPosition);

        IEnemy slingshotCaveman = enemyFactory.CreateEnemy("SlingshotCaveman");
        slingshotCaveman.Position = new Vector2(3, 3);
        slingshotCaveman.AttackClosest(defenders, towerPosition);

        IEnemy shieldCaveman = enemyFactory.CreateEnemy("ShieldCaveman");
        shieldCaveman.Position = new Vector2(0, 1);
        shieldCaveman.AttackClosest(defenders, towerPosition);

        // Defenders attack enemies
        foreach (var defender in defenders)
        {
            defender.Defend(clubCaveman);
            defender.Defend(slingshotCaveman);
            defender.Defend(shieldCaveman);
        }

        
    }

    public void Update()
    {
        towerHealthText.text = $"Tower Health: {tower.Health}";
    }

  

    public void UpdateTowerHealthText()
    {
        towerHealthText.text = $"Tower Health: {tower.Health}";
    }
   
}