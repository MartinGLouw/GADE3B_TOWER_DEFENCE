using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public List<Vector3> path;
    public float speed = 4f;
    private int currentWaypoint = 0;

    void Update()
    {
        if (path == null || currentWaypoint >= path.Count)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 target = path[currentWaypoint];
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position == target)
        {
            currentWaypoint++;
        }
    }
}
