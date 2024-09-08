using System;

public class ShieldCaveman : Enemy
{
    public ShieldCaveman()
    {
        Health = 250;
        Damage = 10;
    }

    public override void Attack()
    {
        Console.WriteLine("ShieldCaveman attacks!");
    }
}