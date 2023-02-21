using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = duration;
        isBuff = (Random.value > 0.5f);
    }

    // Update is called once per frame
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
