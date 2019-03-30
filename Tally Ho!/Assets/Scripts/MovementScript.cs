using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MovementScript : MonoBehaviour
{
    public bool isClimbing;
    public bool onLadder;

    public float speed;
    private Rigidbody2D rb2d;
    private float maxSpeed;
    private Transform lastPlatform;
    private Vector2 stop = new Vector2(0, 0);
    private Vector2 up = new Vector2(0, 2);
    private Vector2 down = new Vector2(0, -2);
    private Vector2 left = new Vector2(-2, 0);
    private Vector2 right = new Vector2(2, 0);

    private Animator anim;
    private SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        speed = 6;
        rb2d = GetComponent<Rigidbody2D>();
        maxSpeed = 3;

        onLadder = false;
        isClimbing = false;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        anim.SetBool("Airborne", !CheckGrounded());
        if (isClimbing)
        {
            rb2d.gravityScale = 0;
        }
        else
        {
            rb2d.gravityScale = 1;
        }

        if(Input.anyKey == false)
        {
            if (isClimbing)
            {
                rb2d.velocity = stop;
            }
            else
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            anim.SetBool("Walking", false);
        }

        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
            if (isClimbing)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rend.flipX = true;
            if (rb2d.velocity.x >= -maxSpeed)
            {
                rb2d.AddForce(Vector3.left * speed);
                anim.SetBool("Walking", true);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rend.flipX = false;
            if (rb2d.velocity.x <= maxSpeed)
            {
                rb2d.AddForce(Vector3.right * speed);
                anim.SetBool("Walking", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (onLadder)
            {
                isClimbing = true;
            }
            if (isClimbing)
            {
                rb2d.velocity = up;
            }
            else if (CheckGrounded()&& !onLadder)
            {
                rb2d.AddForce(Vector3.up * 6, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (onLadder)
            {
                isClimbing = true;
            }
            if (isClimbing)
            {
                rb2d.velocity = down;
            }
            else
            {
                rb2d.AddForce(Vector3.down * speed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D trigCollider)
    {
        if (trigCollider.gameObject.tag == "Ladder")
        {
            onLadder = true;
        }
    }

    private bool CheckGrounded()
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

    private void OnTriggerExit2D(Collider2D trigCollider)
    {
        if (trigCollider.gameObject.tag == "Ladder")
        {
            onLadder = false;
            isClimbing = false;
        }
    }
}
