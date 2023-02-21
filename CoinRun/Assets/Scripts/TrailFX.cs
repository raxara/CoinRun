using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrailFX : MonoBehaviour
{

    Vector3 startTF, targetTF;

    [SerializeField]
    NavMeshAgent agent;

    float distanceToEnd = 0.5f;

    public void Init(Vector3 startTF, Vector3 targetTF)
    {
        this.startTF = startTF;
        this.targetTF = targetTF;
        agent.transform.position = startTF;
        agent.SetDestination(targetTF);
    }

    /*
    private void Update()
    {
        if (distanceToEnd > agent.remainingDistance)
        {
            Destroy(this.gameObject);
        }
    }

    */

}
