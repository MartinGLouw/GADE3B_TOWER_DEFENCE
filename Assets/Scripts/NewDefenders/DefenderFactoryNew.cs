using UnityEngine;

public class DefenderFactoryNew
{
    public enum DefenderType { RaptorDefenderNew, StegoDefenderNew, TrexDefenderNew }

    private static int raptorUpgradeLevel = 0;
    private static int stegoUpgradeLevel = 0;
    private static int trexUpgradeLevel = 0;

    public static T CreateDefender<T>(DefenderType type, Vector3 position) where T : DefenderNew
    {
        GameObject defenderPrefab = null;

        switch (type)
        {
            case DefenderType.RaptorDefenderNew:
                defenderPrefab = Resources.Load<GameObject>("RaptorDefenderNew");
                break;
            case DefenderType.StegoDefenderNew:
                defenderPrefab = Resources.Load<GameObject>("StegoDefenderNew");
                break;
            case DefenderType.TrexDefenderNew:
                defenderPrefab = Resources.Load<GameObject>("TrexDefenderNew");
                break;
            default:
                throw new System.ArgumentException("Invalid defender type");
        }

        if (defenderPrefab != null)
        {
            GameObject defenderObject = Object.Instantiate(defenderPrefab, position, Quaternion.identity);
            T defender = defenderObject.GetComponent<T>();

            ApplyUpgrades(type, defender);
            return defender;
        }
        else
        {
            throw new System.Exception("Failed to load defender prefab.");
        }
    }

    public static void UpgradeDefender(DefenderType type)
    {
        switch (type)
        {
            case DefenderType.RaptorDefenderNew:
                raptorUpgradeLevel++;
                break;
            case DefenderType.StegoDefenderNew:
                stegoUpgradeLevel++;
                break;
            case DefenderType.TrexDefenderNew:
                trexUpgradeLevel++;
                break;
        }
    }

    private static void ApplyUpgrades(DefenderType type, DefenderNew defender)
    {
        switch (type)
        {
            case DefenderType.RaptorDefenderNew:
                for (int i = 0; i < raptorUpgradeLevel; i++) defender.Upgrade();
                break;
            case DefenderType.StegoDefenderNew:
                for (int i = 0; i < stegoUpgradeLevel; i++) defender.Upgrade();
                break;
            case DefenderType.TrexDefenderNew:
                for (int i = 0; i < trexUpgradeLevel; i++) defender.Upgrade();
                break;
        }
    }
}
