using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBeam : MonoBehaviour
{

    [SerializeField]
    Transform aBound, bBound;

    Vector3 center;

    [SerializeField]
    float maxHealth = 10;

    public float curHealth;

    float distBoundToCenter;

    [SerializeField]
    float maxDamagePerSec = 1;

    [SerializeField]
    GameObject beamObj;

    List<GameObject> BaseBeamsTF;

    [SerializeField]
    float resetTimer = 10;

    [SerializeField]
    ParticleSystem dustFX;

    bool isBroken { get {
            return BaseBeamsTF[1].transform.position != beamObj.transform.GetChild(1).transform.position;
        } 
    }

    [SerializeField]
    float impulseStrength = 5;

    // Start is called before the first frame update
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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        dustFX.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        float damage = GetDamage(PlayerController.INSTANCE.transform.position);
        SetDamage(damage);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!IsInLocalSpace(PlayerController.INSTANCE.transform.position))
        {
            curHealth = maxHealth;
        }
        dustFX.Stop();
    }

    float GetDamage(Vector3 playerPos)
    {
        center.y = playerPos.y;
        float dist = Vector3.Distance(playerPos, center);
        float t = Mathf.InverseLerp(distBoundToCenter, 0, dist);
        return maxDamagePerSec * t;
    }

    bool IsInLocalSpace(Vector3 playerPos)
    {
        Vector3 playerLocalPos = transform.InverseTransformPoint(playerPos);
        bool isInXLocalPos = (playerLocalPos.x > -0.45f && playerLocalPos.x < 0.45f);
        bool isInZLocalPos = (playerLocalPos.z > -distBoundToCenter && playerLocalPos.z < distBoundToCenter);
        return isInXLocalPos && isInZLocalPos;
    }
    void SetDamage(float damage)
    {
        if (isBroken) return;
        curHealth -= damage * Time.deltaTime;
        if (curHealth <= 0) BreakBeam();
    }

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
