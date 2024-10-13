using UnityEngine;


public interface IDefender
{
    
    public int MeatCost { get; set; }
    public int Damage { get; set; }
    public virtual void Defend(IEnemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public int Health { get; set; }
    public Vector2 Position { get; }
    float AttRange { get; set; }
}
