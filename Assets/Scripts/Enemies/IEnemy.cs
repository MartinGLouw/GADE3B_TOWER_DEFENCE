using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    void Attack();
    int Health { get; set; }
    Vector2 Position { get; set; }
    void AttackClosest(List<IDefender> defenders, Vector2 towerPosition);
}