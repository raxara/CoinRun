using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostScript : MonoBehaviour
{

    [SerializeField]
    NavMeshAgent agent;

    //liste de points ou se rend le fantome pendant sa patrouille
    [SerializeField]
    List<Transform> patrolPointsList;

    //distance minimum avant laquel le fantome est consideré a destination
    [SerializeField]
    float minDistanceValue = 0.5f;

    //temps d'attente minimum et maximum entre 2 pause pendant la patrouille
    [SerializeField]
    float minPauseDuration = 1;

    [SerializeField]
    float maxPauseDuration = 5;

    //vitesse minimum et maximum du fantome
    [SerializeField]
    float maxSpeed = 5;

    [SerializeField]
    float minSpeed = 1;

    //vitesse pendant le poursuite du joueur
    [SerializeField]
    float chaseSpeed = 4;

    //variable pour jongler entre 2 coroutines différentes
    Coroutine curCorout;

    private void Start()
    {
        curCorout = StartCoroutine(PatrolCorout());
    }

    //coroutine de patrouille
    IEnumerator PatrolCorout()
    {
        int curTargetTFIndex = -1;
        Vector3 center;
        float midDist = 0;
        while (true)
        {
            //on choisit une destination au hasard, en evitant la destination actuelle
            float tempIndex = curTargetTFIndex;
            while (tempIndex == curTargetTFIndex) tempIndex = Random.Range(0, patrolPointsList.Count);
            Vector3 targetTF = patrolPointsList[Random.Range(0, patrolPointsList.Count)].position;
            //on fait les calculs qui serviront a modifier la vitesse du joueur
            center = (transform.position + targetTF) * 0.5f;
            midDist = Vector3.Distance(transform.position, targetTF) * 0.5f;
            agent.SetDestination(targetTF);
            yield return null;
            //on met a jour la vitesse du joueur 
            while (agent.remainingDistance > minDistanceValue)
            {
                updateSpeed(transform.position, center, midDist);
                yield return null;
            }
            //pause du fantome
            float t = Random.Range(minPauseDuration, maxPauseDuration);
            while (t > 0)
            {
                t -= Time.deltaTime;
                yield return null;
            }
            
        }
        
    }

    //coroutine de poursuite du joueur
    IEnumerator ChaseCorout(GameObject player)
    {
        while (true)
        {
            agent.SetDestination(player.transform.position);
            yield return null;
        }
    }

    //fonction qui modifie la vitesse du fantome dependant de sa proximité du centre de son parcours 
    //manque un peu de parametrage
    void updateSpeed(Vector3 playerPos, Vector3 center, float midDist)
    {
        center.y = playerPos.y;
        float dist = Vector3.Distance(playerPos, center);
        float t = Mathf.InverseLerp(midDist, 0, dist);
        t = Mathf.Clamp(t *= 2, 0, 1);
        float speed = Mathf.Clamp(maxSpeed * t, minSpeed, maxSpeed);
        agent.speed = speed;
    }

    //fonctions utilisées par le script GhostDetectionZone pour passer de la patrouille a la poursuite et vice versa
    public void FocusPlayer(GameObject player)
    {
        StopCoroutine(curCorout);
        agent.speed = chaseSpeed;
        curCorout = StartCoroutine(ChaseCorout(player));
    }

    public void StartPatrolling()
    {
        StopCoroutine(curCorout);
        curCorout = StartCoroutine(PatrolCorout());
    }

}
