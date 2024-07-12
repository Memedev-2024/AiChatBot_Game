using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isPlayer1;
    public float moveSpeed;
    public Rigidbody2D rb;

    private Vector2 movement;
    private Vector3 startPosition;

    //AI Varies
    public bool isAIControlled;
    public GameObject ball;
    public float detectionRange = 5f;
    public float smoothTime = 0.2f;
    private Vector2 targetPosition;

    public void Reset()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = Vector2.zero;
        //PlayerInpt
        if (isPlayer1 == true)
        {
            //Get Player1 Input
            if (Input.GetKey(KeyCode.A))
            {
                movement.x = -1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement.x = 1;
            }

        }
        else
        {
            if (isAIControlled)
            {
                // AIControll
                AIControl();
            }
            else
            {
                // Get Player2 Input
                if (Input.GetKey(KeyCode.L))
                {
                    movement.x = 1;
                }
                if (Input.GetKey(KeyCode.J))
                {
                    movement.x = -1;
                }
            }
        }

        //RigidBodyController
        rb.velocity = movement* moveSpeed;



        void AIControl()
        {
            float ballPositionX = ball.transform.position.x;
            float paddlePositionX = transform.position.x;
            float distanceToBall = Mathf.Abs(ball.transform.position.y - transform.position.y);

            if (ball != null)
            {
                // Only move in detected range
                if (distanceToBall <= detectionRange)
                {
                    if (ballPositionX > paddlePositionX)
                    {
                        targetPosition.x = 1;
                    }
                    else if (ballPositionX < paddlePositionX)
                    {
                        targetPosition.x = -1;
                    }
                    else
                    {
                        targetPosition.x = 0;
                    }
                }
                else
                {
                    targetPosition.x = 0;; //Out of deection
                }
                movement = Vector2.Lerp(movement, targetPosition, smoothTime);
                rb.velocity = movement * moveSpeed;
            }
        }
    }
}
