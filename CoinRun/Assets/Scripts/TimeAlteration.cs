using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//alteration qui ajoute ou enleve du temps au joueur
public class TimeAlteration : MonoBehaviour
{
    [SerializeField]
    int seconds;

    [SerializeField]
    Timer timer;

    bool isBuff;

    // Start is called before the first frame update
    void Start()
    {
        isBuff = (Random.value > 0.5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            activateAlteration();
            Destroy(gameObject);
        }
    }

    void activateAlteration()
    {
        if (isBuff) timer.timeRemaining += seconds;
        else timer.timeRemaining -= seconds;

    }
}
