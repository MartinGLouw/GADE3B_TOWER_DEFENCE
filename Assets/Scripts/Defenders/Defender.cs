using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Defender : MonoBehaviour, IDefender
{
    public virtual int MeatCost { get; set; }
    public int Damage { get; set; }
    public virtual void Defend(IEnemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public int Health { get; set; }
    public Vector2 Position { get; set; }
    
    public float AttRange { get; set; }
}
