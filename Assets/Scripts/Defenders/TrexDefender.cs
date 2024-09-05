using System;
using UnityEngine;

public class TrexDefender : IDefender
{
    public int Health { get; set; } = 150;
    public Vector2 Position { get; set; }

    public override void Defend(IEnemy enemy)
    {
        Console.WriteLine("Raptor Bites");
        enemy.Health -= 20; // Example damage value
    }
}
