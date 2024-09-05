using System;
using UnityEngine;

public class EnemyFactory
{
    public IEnemy CreateEnemy(string enemyType)
    {
        switch (enemyType)
        {
            case "ClubCaveman":
                return new ClubCaveman();
            case "SlingshotCaveman":
                return new SlingshotCaveman();
            case "ShieldCaveman":
                return new ShieldCaveman();
            default:
                throw new ArgumentException("Invalid enemy type");
        }
    }
}
