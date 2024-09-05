using System;
using UnityEngine;

public class StegoDefender : IDefender
{
    public int Health { get; set; } = 200;
    public Vector2 Position { get; set; }

    public override void Defend(IEnemy enemy)
    {
        Console.WriteLine("Raptor Bites");
        enemy.Health -= 25; // Example damage value
    }
}
