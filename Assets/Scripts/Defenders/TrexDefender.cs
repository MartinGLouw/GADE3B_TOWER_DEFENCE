using System;
using UnityEngine;

public class TrexDefender : IDefender
{
    public TrexDefender()
    {
        Health = 300;
        MeatCost = 200;
        Damage = 60;

    }
    public Vector2 Position { get; set; }

    public override void Defend(IEnemy enemy)
    {
        Console.WriteLine("Raptor Bites");
        enemy.Health -= Damage; 
    }
}
