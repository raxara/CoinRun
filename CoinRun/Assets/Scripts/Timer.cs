using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//script du timer
public class Timer : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI timerText;

    [HideInInspector]
    public float timeRemaining;

    [SerializeField]
    private float StartingTimer = 120;

    //le timer n'utilise qu'une coroutine
    void Start()
    {
        timeRemaining = StartingTimer;
        StartCoroutine(TimerCorout());
    }

    IEnumerator TimerCorout()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            display(timeRemaining);
            yield return null;
        }
        display(0);
        LevelManager.INSTANCE.GameOver();

    }

    // format : MM:SS:MS
    private void display(float timer)
    {
        timerText.text = Mathf.FloorToInt(timer / 60) + ":" + Mathf.FloorToInt(timer % 60) + ":" + Mathf.FloorToInt(timer * 1000 % 1000);
    }
}
