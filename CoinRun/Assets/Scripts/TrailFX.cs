using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//script de l'orbe
public class TrailFX : MonoBehaviour
{

    Vector3 startTF, targetTF;

    [SerializeField]
    NavMeshAgent agent;

    public void Init(Vector3 startTF, Vector3 targetTF)
    {
        this.startTF = startTF;
        this.targetTF = targetTF;
        agent.transform.position = startTF;
        agent.SetDestination(targetTF);
    }

}
