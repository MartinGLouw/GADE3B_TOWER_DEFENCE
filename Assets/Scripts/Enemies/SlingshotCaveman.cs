using System;


public class SlingshotCaveman : Enemy
{
    public SlingshotCaveman()
    {
        Health = 150;
        Damage = 20;
        AttRange = 20;
    }

    public override void Attack()
    {
        Console.WriteLine("SlingshotCaveman attacks!");
    }
}