using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//buff ou debuff qui modifie la vitesse du joueur
public class SpeedAlteration : MonoBehaviour
{
    [SerializeField]
    float speedMultiplicator = 1.2f;

    [SerializeField]
    float duration = 5;

    float timeRemaining;

    bool isBuff;

    CharacterMovement charMov;

    public MeshRenderer meshRend;

    public Collider col;


    // on choisit au hasard si c'est un buff ou un debuff
    void Start()
    {
        timeRemaining = duration;
        isBuff = (Random.value > 0.5f);
    }

    // le joueur prend l'alteration, on lance la coroutine
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            meshRend.enabled = false;
            charMov = other.gameObject.GetComponent<CharacterMovement>();
            StartCoroutine(TimerCorout());
            col.enabled = false;
        }
    }

    //suivant si l'alteration est un buff ou un debuf, on multiplie ou divise la vitesse par le modifier, puis on la desactive en faisant le calcul inverse
    void activateAlteration(bool activate)
    {
        if (isBuff)
        {
            if (activate) charMov.m_moveSpeed *= speedMultiplicator;
            else charMov.m_moveSpeed /= speedMultiplicator;
        }
        else
        {
            if (activate) charMov.m_moveSpeed /= speedMultiplicator;
            else charMov.m_moveSpeed *= speedMultiplicator;
        }
        
    }

    //la coroutine active l'alteration, attend un certain temps et la desactive avant de detruire l'objet actif
    IEnumerator TimerCorout()
    {
        activateAlteration(true);
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        activateAlteration(false);
        Destroy(gameObject);
    }
}
