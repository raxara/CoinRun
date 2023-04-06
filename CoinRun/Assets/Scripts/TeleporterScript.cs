using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScript : MonoBehaviour
{

    [SerializeField]
    Transform tp_Destination;

    [SerializeField]
    float animationTime = 1f;

    [SerializeField]
    float maxAnimationWidth = 1.5f;

    [SerializeField]
    float minAnimationHeight = 0.5f;

    [SerializeField]
    float maxAnimationHeight = 2f;

    [SerializeField]
    Transform playerTF;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(AnimationCorout());
        }
    }

    private void TeleportPlayer()
    {
        playerTF.position = tp_Destination.position;
        playerTF.localScale = new Vector3(1, 1, 1);
    }

    //animation via code, se fait en 2 temps, le premier "squeeze" puis l'"affinage"
    IEnumerator AnimationCorout()
    {
        float curTime = 0;
        Vector3 midTF = new Vector3(playerTF.localScale.x * maxAnimationWidth, playerTF.localScale.y * minAnimationHeight, playerTF.localScale.z * maxAnimationWidth);
        Vector3 endTF = new Vector3(0, playerTF.localScale.y * maxAnimationHeight, 0);
        while (curTime <= animationTime)
        {
            playerTF.localScale = Vector3.Lerp(playerTF.localScale, midTF, curTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        curTime = 0;
        while (curTime <= animationTime)
        {
            playerTF.localScale = Vector3.Lerp(playerTF.localScale, endTF, curTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        TeleportPlayer();
    }
}
