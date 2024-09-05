using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DefenderFactory : MonoBehaviour
{
    public IDefender CreateDefender(string defenderType)
    {
        switch (defenderType)
        {
            case "StegoDefender":
                return new StegoDefender();
            case "RaptorDefender":
                return new RaptorDefender();
            case "TrexDefender":
                return new TrexDefender();
            default:
                throw new ArgumentException("Invalid defender type");
        }
    }
}
