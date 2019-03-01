using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb2d;
    public bool isJumping;
    private Vector2 grounded;
    private float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6;
        rb2d = GetComponent<Rigidbody2D>();
        isJumping = false;
        grounded = new Vector2(0, 0.5f);
        maxSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        isJumping = rb2d.position.y >= grounded.y;
        
        if (Input.GetKey(KeyCode.None))
        {
            //
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (rb2d.velocity.x <= maxSpeed)
            {
                rb2d.AddForce(Vector3.left * speed);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (rb2d.velocity.x <= maxSpeed)
            {
                rb2d.AddForce(Vector3.right * speed);
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (!isJumping) {
                rb2d.AddForce(Vector3.up * 2, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2d.AddForce(Vector3.down * speed);
        }
    }
}
