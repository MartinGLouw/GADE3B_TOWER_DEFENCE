using System;
using UnityEngine;

public class RaptorDefender : IDefender
{
    public RaptorDefender()
    {
        Health = 100;
        MeatCost = 20;
        Damage = 50;

    }
    
    public Vector2 Position { get; set; }

    public override void Defend(IEnemy enemy)
    {
        Console.WriteLine("Raptor Bites");
        enemy.Health -= Damage; 
    }
}
