using System;

public class ShieldCaveman : Enemy
{
    public ShieldCaveman()
    {
        Health = 100;
    }

    public override void Attack()
    {
        Console.WriteLine("SlingshotCaveman attacks!");
    }
}