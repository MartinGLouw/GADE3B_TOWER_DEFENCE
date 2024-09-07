using System;

public class ClubCaveman : Enemy
{
    public ClubCaveman()
    {
        Health = 100;
        Damage = 30;
    }

    public override void Attack()
    {
        Console.WriteLine("ClubCaveman attacks!");
    }
}