using System;
using UnityEngine;

public class StegoDefender : IDefender
{
    public StegoDefender()
    {
        Health = 200;
        MeatCost = 60;
        Damage = 25;
        

    }
    public Vector2 Position { get; set; }

    public override void Defend(IEnemy enemy)
    {
        Console.WriteLine("Raptor Bites");
        enemy.Health -= Damage; 
    }
}
