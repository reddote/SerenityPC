using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainHugging : MonoBehaviour
{

    
    RaycastHit hit;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
    }


    private void LateUpdate()
    {
        if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 5))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal), hit.normal), Time.deltaTime * 5.0f);
            transform.position = agent.transform.position;
        }
    }


}
