using UnityEngine;


public class IDefender
{
    public virtual void Defend(IEnemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public int Health { get; set; }
    public Vector2 Position { get; }
}
