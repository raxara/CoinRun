using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{

    [SerializeField]
    NavMeshAgent agent;

    [SerializeField]
    List<Transform> patrolPointsList;

    [SerializeField]
    float minDistanceValue = 0.5f;

    [SerializeField]
    float minPauseDuration = 1;

    [SerializeField]
    float maxPauseDuration = 5;

    [SerializeField]
    float maxSpeed = 5;

    [SerializeField]
    float minSpeed = 1;

    private void Start()
    {
        StartCoroutine(PatrolCorout());
    }

    IEnumerator PatrolCorout()
    {
        int curTargetTFIndex = -1;
        Vector3 center;
        float midDist = 0;
        while (true)
        {
            float tempIndex = curTargetTFIndex;
            while (tempIndex == curTargetTFIndex) tempIndex = Random.Range(0, patrolPointsList.Count);
            Vector3 targetTF = patrolPointsList[Random.Range(0, patrolPointsList.Count)].position;
            center = (transform.position + targetTF) * 0.5f;
            midDist = Vector3.Distance(transform.position, targetTF) * 0.5f;
            agent.SetDestination(targetTF);
            yield return null;
            while (agent.remainingDistance > minDistanceValue)
            {
                updateSpeed(transform.position, center, midDist);
                yield return null;
            }
            /*
            float t = Random.Range(minPauseDuration, maxPauseDuration);
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            */
        }
        
    }

    void updateSpeed(Vector3 playerPos, Vector3 center, float midDist)
    {
        center.y = playerPos.y;
        float dist = Vector3.Distance(playerPos, center);
        float t = Mathf.InverseLerp(midDist, 0, dist);
        t = Mathf.Clamp(t *= 2, 0, 1);
        float speed = Mathf.Clamp(maxSpeed * t, minSpeed, maxSpeed);
        agent.speed = speed;
    }


}
