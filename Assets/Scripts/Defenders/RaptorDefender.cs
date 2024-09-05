using System;
using UnityEngine;

public class RaptorDefender : IDefender
{
    public int Health { get; set; } = 100;
    public Vector2 Position { get; set; }

    public override void Defend(IEnemy enemy)
    {
        Console.WriteLine("Raptor Bites");
        enemy.Health -= 50; // Example damage value
    }
}
