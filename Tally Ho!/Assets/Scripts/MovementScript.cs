using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb2d;
    private float maxSpeed;
    private Transform lastPlatform;

    // Start is called before the first frame update
    void Start()
    {
        speed = 6;
        rb2d = GetComponent<Rigidbody2D>();
        maxSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
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

        bool CheckGrounded()
        {
            RaycastHit2D[] thingIHit = new RaycastHit2D[1];
            bool grounded = GetComponent<Rigidbody2D>().Cast(Vector2.down, thingIHit, 0.02f) > 0;
            if (grounded)
            {
                string tag = thingIHit[0].transform.gameObject.tag;
                if (tag.Equals("Platform"))
                {
                    lastPlatform = thingIHit[0].transform;
                }
            }

            return grounded;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CheckGrounded())
            {
                rb2d.AddForce(Vector3.up * 6, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb2d.AddForce(Vector3.down * speed);
        }
    }
}
