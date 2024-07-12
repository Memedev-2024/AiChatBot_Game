using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PingPongMiniGame : MonoBehaviour
{
    [Header("Ball")]
    public GameObject ball;

    [Header("Player1")]
    public GameObject player1;
    public GameObject player1Goal;

    [Header("Player2")]
    public GameObject player2;
    public GameObject player2Goal;

    [Header("ScoreUI")]
    public GameObject scoreUI_1;
    public GameObject scoreUI_2;

    private int player1Score;
    private int player2Score;
    

    public void Player1Scored()
    {
        player1Score++;
        scoreUI_1.GetComponent<TextMeshProUGUI>().text = player1Score.ToString();
        ResetGame();
    }

    public void Player2Scored()
    {
        player2Score++;
        scoreUI_2.GetComponent<TextMeshProUGUI>().text = player2Score.ToString();
        ResetGame();

    }
    private void ResetGame()
    {
        ball.GetComponent<Ball>().Reset();
        player1.GetComponent<Paddle>().Reset(); 
        player2.GetComponent<Paddle>().Reset();

        ball.GetComponent<Ball>().Launch();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
       
    }
}
