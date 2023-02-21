using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI timerText;

    [HideInInspector]
    public float timeRemaining;

    [SerializeField]
    private float StartingTimer = 120;

    // Start is called before the first frame update
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

    private void display(string message)
    {
        timerText.text = message;
    }
}
