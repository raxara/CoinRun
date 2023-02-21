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

    float curHealth;

    float distBoundToCenter;

    [SerializeField]
    float maxDamagePerSec = 1;


    // Start is called before the first frame update
    void Start()
    {
        center = (aBound.position + bBound.position) * 0.5f;
        distBoundToCenter = Vector3.Distance(center, aBound.position);
        curHealth = maxHealth;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        float damage = GetDamage(PlayerController.INSTANCE.transform.position);
        SetDamage(damage);
        Debug.Log(damage + ", " + curHealth);
    }

    float GetDamage(Vector3 playerPos)
    {
        center.y = playerPos.y;
        float dist = Vector3.Distance(playerPos, center);
        float t = Mathf.InverseLerp(distBoundToCenter, 0, dist);
        return maxDamagePerSec * t;
    }

    void SetDamage(float damage)
    {
        curHealth -= damage * Time.deltaTime;
        if (curHealth <= 0) BreakBeam();
    }

    void BreakBeam()
    {
        gameObject.SetActive(false);
    }

}
