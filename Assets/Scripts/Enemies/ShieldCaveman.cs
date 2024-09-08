using System;

public class ShieldCaveman : Enemy
{
    public ShieldCaveman()
    {
        Health = 250;
        Damage = 10;
        AttRange = 20;
    }

    public override void Attack()
    {
        
        Console.WriteLine("SlingshotCaveman attacks!");
    }
}