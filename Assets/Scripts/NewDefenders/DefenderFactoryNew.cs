using UnityEngine;

public class DefenderFactoryNew
{
    public enum DefenderType { RaptorDefenderNew, StegoDefenderNew, TrexDefenderNew }

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
            return defenderObject.GetComponent<T>();
        }
        else
        {
            throw new System.Exception("Failed to load defender prefab.");
        }
    }
}