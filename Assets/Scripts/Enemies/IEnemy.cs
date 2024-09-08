using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public interface IEnemy
{
    int Damage { get; set; }
    void Attack();
    int Health { get; set; }
    Vector2 Position { get; set; }
    void AttackClosest(List<IDefender> defenders, Vector2 towerPosition);
    float AttRange { get; set; }

   
}