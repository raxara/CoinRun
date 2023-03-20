using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    List<Vector3> patrolPointsList;

    private void Start()
    {
        StartCoroutine(PatrolCorout());
    }

    IEnumerator PatrolCorout()
    {
        while (true)
        {
            Vector3 targetTF = patrolPointsList[Random.Range(0, patrolPointsList.Count)];
            agent.SetDestination(targetTF);
            while (agent.pathStatus != NavMeshPathStatus.PathComplete) yield return null;
        }
    }

}
