using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MovementScript : MonoBehaviour {
    public bool isClimbing;
    //public bool onLadder;


    private int ladderDepth;
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

    public Transform punchbox;
    public Transform kickbox;//heh
    // Start is called before the first frame update
    void Start() {
        speed = 300;
        rb2d = GetComponent<Rigidbody2D>();
        maxSpeed = 3;

        ladderDepth = 0;
        isClimbing = false;
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

        anim.SetBool("Airborne", !CheckGrounded());
        anim.SetBool("Climbing", isClimbing);
        if (isClimbing) {
            rb2d.gravityScale = 0;
        } else {
            rb2d.gravityScale = 1;
        }

        if (Input.anyKey == false) {
            if (isClimbing) {
                anim.SetBool("Climbing-Paused", true);
                rb2d.velocity = stop;
            } else {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            anim.SetBool("Walking", false);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) {
            if (isClimbing) {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
        }

        if (Input.GetKey(KeyCode.A)) {
            rend.flipX = true;
            flipBoxes(rend.flipX);
            if (rb2d.velocity.x >= -maxSpeed) {
                rb2d.AddForce(Vector3.left * Time.deltaTime * speed);
                anim.SetBool("Walking", true);
                anim.SetBool("Climbing-Paused", false);
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            rend.flipX = false;
            flipBoxes(rend.flipX);
            if (rb2d.velocity.x <= maxSpeed) {
                rb2d.AddForce(Vector3.right * Time.deltaTime * speed);
                anim.SetBool("Walking", true);
                anim.SetBool("Climbing-Paused", false);
            }
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            if (onLadder()) {
                isClimbing = true;
            }
            if (isClimbing) {
                anim.SetBool("Climbing-Paused", false);
                rb2d.velocity = up;
            } else if (CheckGrounded() && !onLadder()) {
                rb2d.AddForce(Vector3.up * 6, ForceMode2D.Impulse);
            }
        }

        if (Input.GetKey(KeyCode.S)) {
            if (onLadder()) {
                isClimbing = true;
            }
            if (isClimbing) {
                anim.SetBool("Climbing-Paused", false);
                rb2d.velocity = down;
            } else {
                rb2d.AddForce(Vector3.down * Time.deltaTime * speed);
            }
        }

        //Debug.Log(anim.GetBool("Climbing-Paused"));
    }

    private bool onLadder() {
        return ladderDepth > 0;
    }

    private void OnTriggerEnter2D(Collider2D trigCollider) {
        if (trigCollider.gameObject.tag == "Ladder") {
            ladderDepth++;
        }
    }

    private bool CheckGrounded() {
        RaycastHit2D[] thingIHit = new RaycastHit2D[1];
        LayerMask groundmask = LayerMask.GetMask("Ground");
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = groundmask;
        bool grounded = GetComponent<Rigidbody2D>().Cast(Vector2.down, filter, thingIHit, 0.02f) > 0;
        if (grounded) {
            string tag = thingIHit[0].transform.gameObject.tag;
            if (tag.Equals("Ladder")) {
                //Not Grounded.
                //return false;
            }
        }

        return grounded;
    }

    private void OnTriggerExit2D(Collider2D trigCollider) {
        if (trigCollider.gameObject.tag == "Ladder") {
            ladderDepth--;
            isClimbing = onLadder();
        }
    }

    private void flipBoxes(bool flipped) {
        if (flipped && Mathf.Sign(punchbox.localPosition.x) > 0) {
            punchbox.localPosition = new Vector3(-punchbox.localPosition.x, punchbox.localPosition.y, punchbox.localPosition.z);
            kickbox.localPosition = new Vector3(-kickbox.localPosition.x, kickbox.localPosition.y, kickbox.localPosition.z);
        } else if (!flipped && Mathf.Sign(punchbox.localPosition.x) < 0) {
            punchbox.localPosition = new Vector3(-punchbox.localPosition.x, punchbox.localPosition.y, punchbox.localPosition.z);
            kickbox.localPosition = new Vector3(-kickbox.localPosition.x, kickbox.localPosition.y, kickbox.localPosition.z);
        }
    }
}
