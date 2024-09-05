using System;


public class SlingshotCaveman : Enemy
{
    public SlingshotCaveman()
    {
        Health = 100;
    }

    public override void Attack()
    {
        Console.WriteLine("SlingshotCaveman attacks!");
    }
}