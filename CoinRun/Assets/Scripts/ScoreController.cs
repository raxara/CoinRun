using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{

    public static ScoreController INSTANCE;

    private int score;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Awake()
    {
        INSTANCE = this;
    }


    public static void AddCoin(int coinValue)
    {
        INSTANCE.score += coinValue;
        INSTANCE.UpdateScoreUI();
    }

    public static void removeCoin(int coinValue)
    {
        INSTANCE.score -= coinValue;
        INSTANCE.UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = INSTANCE.score.ToString(); 
    }


}
