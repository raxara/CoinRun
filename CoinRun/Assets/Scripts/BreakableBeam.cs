using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script de destruction d'une poutre qui sert de pont
public class BreakableBeam : MonoBehaviour
{
    //limites, centre de la poutre et distance de l'un a l'autre, servent a calculer la perte de solidité
    [SerializeField]
    Transform aBound, bBound;

    Vector3 center;

    float distBoundToCenter;

    //solidité max et actuelle de la poutre, une fois a 0, la poutre se casse
    [SerializeField]
    float maxHealth = 10;

    public float curHealth;

    //"degats" maximum (au centre) que prendra la poutre par seconde
    [SerializeField]
    float maxDamagePerSec = 1;

    //references a l'objet et les elements qui la composent
    [SerializeField]
    GameObject beamObj;

    List<GameObject> BaseBeamsTF;

    //temps entre la destruction de la poutre et son respawn
    [SerializeField]
    float resetTimer = 10;

    //reference de la fx de poussiere
    [SerializeField]
    ParticleSystem dustFX;

    //force a utiliser lors de la destruction de la poutre
    [SerializeField]
    float impulseStrength = 5;

    //propriété evitant des redondances
    bool isBroken { 
        get {
            return BaseBeamsTF[1].transform.position != beamObj.transform.GetChild(1).transform.position;
        } 
    }
    
    //calcul et affectation des differentes variables
    void Start()
    {
        center = (aBound.position + bBound.position) * 0.5f;
        distBoundToCenter = Vector3.Distance(center, aBound.position);
        curHealth = maxHealth;
        BaseBeamsTF = new List<GameObject>();
        foreach (Transform child in beamObj.transform)
        {
            GameObject obj = new GameObject();
            obj.transform.rotation = child.transform.rotation;
            obj.transform.position = child.position;
            BaseBeamsTF.Add(obj);
        }
    }

    //si le joueur est en contact avec la poutre, elle fait de la poussiere
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        dustFX.Play();
    }

    //si le joueur reste sur la poutre, elle prendra des dommages periodiquement
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        float damage = GetDamage(PlayerController.INSTANCE.transform.position);
        SetDamage(damage);
    }

    //des que le joueur sort du trigger de la poutre, on desactive la fx et on reinitialise la vie de la poutre
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!IsInLocalSpace(PlayerController.INSTANCE.transform.position))
        {
            curHealth = maxHealth;
        }
        dustFX.Stop();
    }

    //fonction permettant de calculer des degats par seconde grace a un reverseLerp (minimum sur les cotés, maximum au centre)
    float GetDamage(Vector3 playerPos)
    {
        center.y = playerPos.y;
        float dist = Vector3.Distance(playerPos, center);
        float t = Mathf.InverseLerp(distBoundToCenter, 0, dist);
        return maxDamagePerSec * t;
    }

    //fonction permettant de verifier si le joueur est encore sur la poutre, meme pendant un saut
    bool IsInLocalSpace(Vector3 playerPos)
    {
        Vector3 playerLocalPos = transform.InverseTransformPoint(playerPos);
        bool isInXLocalPos = (playerLocalPos.x > -0.45f && playerLocalPos.x < 0.45f);
        bool isInZLocalPos = (playerLocalPos.z > -distBoundToCenter && playerLocalPos.z < distBoundToCenter);
        return isInXLocalPos && isInZLocalPos;
    }

    //fonction qui applique les degats a la poutre
    void SetDamage(float damage)
    {
        if (isBroken) return;
        curHealth -= damage * Time.deltaTime;
        if (curHealth <= 0) BreakBeam();
    }

    //fonction qui detruit la poutre en desactivant la gravité sur tous les differents bouts la composant puis lance la coroutine qui la reinitialisera
    void BreakBeam()
    {
        if (isBroken) return;
        foreach (Transform child in beamObj.transform)
        {
            child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        beamObj.transform.GetChild(1).GetComponent<Rigidbody>().AddForce(Vector3.down * impulseStrength, ForceMode.Impulse);
        StartCoroutine(ResetTimerCorout());
    }

    //Coroutine qui reinitialise la poutre apres un certain temps
    IEnumerator ResetTimerCorout()
    {
        float curTimer = 0;
        while (curTimer < resetTimer)
        {
            curTimer += Time.deltaTime;
            yield return null;
        }
        ResetBeam();
    }

    //fonction qui reconstruit la poutre
    void ResetBeam()
    {
        curHealth = maxHealth;
        for (int i = 0; i < beamObj.transform.childCount; i++)
        {
            GameObject beamChild = beamObj.transform.GetChild(i).gameObject;
            beamChild.GetComponent<Rigidbody>().isKinematic = true;
            beamChild.transform.position = BaseBeamsTF[i].transform.position;
            beamChild.transform.rotation = BaseBeamsTF[i].transform.rotation;
        }
    }

}
