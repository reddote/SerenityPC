using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshHitUtility : MonoBehaviour
{
    /// <summary>
	/// Get a random point on the NavMesh within the given center and range.
	/// </summary>
	public static Vector3 RandomPointOnNavMesh(NavMeshHit hit, Vector3 center, float range)
    {
        Vector3 result = Vector3.zero;

        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                break;
            }
        }

        return result;
    }

}
