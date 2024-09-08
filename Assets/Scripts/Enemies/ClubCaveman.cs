using System;

public class ClubCaveman : Enemy
{
    public ClubCaveman()
    {
        Health = 100;
        Damage = 30;
        AttRange = 20;
    }

    public override void Attack()
    {
        Console.WriteLine("ClubCaveman attacks!");
    }
    
}