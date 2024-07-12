using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    private Vector3 startPosition;
    public float hitForce = 100f;

    //Launch the ball 2 ramdom vector
    public void Reset()
    {
        rb.velocity = Vector2.zero;
        transform.position = startPosition;
    }
    public void Launch()
    {
        float x = Random.Range(-1f, 1f);
        float yDirection = Random.Range(0, 2) == 0 ? 1f : -1f; // 随机选择向上还是向下
        float y = yDirection * Random.Range(0.5f, 1f); // 随机生成一个小于1的数
        rb.velocity = new Vector2(x, y) * speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        Launch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {

            // Apply a random force when hitting the paddle
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 randomForce = randomDirection * hitForce;

            rb.velocity += randomForce;
        }
    }
}
