using UnityEngine;

public class EnemyFactoryNew : MonoBehaviour
{
    public GameObject proceduralEnemyPrefab; 

    public enum EnemyType
    {
        ClubCavemanNew,
        ShieldCavemanNew,
        NetCavemanNew,
        ProceduralEnemy 
    }

    public static T CreateEnemy<T>(EnemyType type, Vector3 position) where T : MonoBehaviour
    {
        GameObject enemyPrefab = null;

        switch (type)
        {
            case EnemyType.ClubCavemanNew:
                enemyPrefab = Resources.Load<GameObject>("ClubCavemanNew");
                break;
            case EnemyType.ShieldCavemanNew:
                enemyPrefab = Resources.Load<GameObject>("ShieldCavemanNew");
                break;
            case EnemyType.NetCavemanNew:
                enemyPrefab = Resources.Load<GameObject>("NetCavemanNew");
                break;
            case EnemyType.ProceduralEnemy:
                enemyPrefab = Resources.Load<GameObject>("ProceduralEnemy");
                break;
            default:
                throw new System.ArgumentException("Invalid enemy type");
        }

        if (enemyPrefab != null)
        {
            GameObject enemyObject = Object.Instantiate(enemyPrefab, position, Quaternion.identity);
            return enemyObject.GetComponent<T>();
        }
        else
        {
            throw new System.Exception("Failed to load enemy prefab.");
        }
    }
}