using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : IEnemy
{
    public int Health { get; set; }
    public Vector2 Position { get; set; }

    public abstract void Attack();

    public void AttackClosest(List<IDefender> defenders, Vector2 towerPosition)
    {
        IDefender closestDefender = null;
        float closestDistance = float.MaxValue;

        foreach (var defender in defenders)
        {
            float distance = Vector2.Distance(Position, defender.Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestDefender = defender;
            }
        }

        float distanceToTower = Vector2.Distance(Position, towerPosition);
        if (distanceToTower < closestDistance)
        {
            // Attack the tower
            Console.WriteLine($"{GetType().Name} attacks the tower!");
        }
        else if (closestDefender != null)
        {
            // Attack the closest defender
            Console.WriteLine($"{GetType().Name} attacks the closest defender!");
            closestDefender.Health -= 10; // Example damage value
        }
    }
}