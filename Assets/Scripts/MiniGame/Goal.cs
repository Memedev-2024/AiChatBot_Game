using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool isPlayer1Goal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if it is ball
        if (collision.gameObject.CompareTag("Ball"))
        {

            if (isPlayer1Goal)
            {
                // Debug.Log("Player 1's goal was hit!");
                GameObject.Find("PingPongMiniGameManager").GetComponent<PingPongMiniGame>().Player1Scored();

            }
            else
            {
                // Debug.Log("Player 2's goal was hit!");
                GameObject.Find("PingPongMiniGameManager").GetComponent<PingPongMiniGame>().Player2Scored();

            }


            
        }

    }
}

