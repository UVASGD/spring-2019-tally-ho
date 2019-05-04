using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum States {
    idle,
    approach,
    attack,
    wait,
    retreat
}

public class SmartBoy : MonoBehaviour
{

    GameObject player;

    Rigidbody2D rb;

    States state;

    float timer;

    SpriteRenderer sprite;

    Animator anim;

    public float maxSpeed;
    public float force;
    public float attackDistance;

    public Transform punchbox;

    float epsilon = 0.1f;

    Dictionary<States, float> timeouts = new Dictionary<States, float>() {
        {States.idle, 2.0f},
        {States.approach, 0.0f},
        {States.attack, 0.0f},
        {States.wait, 1.5f},
        {States.retreat, 1.0f},
    };

    Dictionary<States, bool> doesTimeout = new Dictionary<States, bool>() {
        {States.idle, true},
        {States.approach, false},
        {States.attack, false},
        {States.wait, true},
        {States.retreat, true},
    };

    Dictionary<States, States> nextState = new Dictionary<States, States>() {
        {States.idle, States.approach},
        {States.approach, States.attack},
        {States.attack, States.wait},
        {States.wait, States.retreat},
        {States.retreat, States.idle},
    };

    // Start is called before the first frame update
    void Start()
    {
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        state = States.idle;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(state + " : time = " + timer);

        timer += Time.deltaTime;

        if(doesTimeout[state] && timer > timeouts[state]) {
            advanceState();
        }

        if(rb.velocity.x > epsilon) {
            setDirection(false);
        } else if(rb.velocity.x < -epsilon) {
            setDirection(true);
        }

        if(Mathf.Abs(rb.velocity.x) > epsilon) {
            anim.SetBool("walking", true);
        } else {
            anim.SetBool("walking", false);
        }

        switch (state){
            case States.idle:
                //do nothing.
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
            case States.approach:
                //move towards player
                Vector2 godelta = player.transform.position - gameObject.transform.position;
                if(rb.velocity.magnitude <= maxSpeed || Vector2.Dot(rb.velocity, godelta) < 0) {
                    rb.AddForce(Vector2.Dot(godelta, Vector2.right) * Vector2.right * force, ForceMode2D.Impulse);
                }

                if(godelta.magnitude <= attackDistance) {
                    advanceState();
                }
                break;
            case States.attack:
                //attack!
                anim.SetTrigger("punch");
                advanceState();
                break;
            case States.wait:
                //pause for reflection...
                break;
            case States.retreat:
                //run away!!!!
                Vector2 rundelta = player.transform.position - gameObject.transform.position;
                if (rb.velocity.magnitude <= maxSpeed || Vector2.Dot(rb.velocity, rundelta) > 0) {
                    rb.AddForce(Vector2.Dot(rundelta, Vector2.right) * Vector2.right * force * -1, ForceMode2D.Impulse);
                }
                break;
        }

    }


    void advanceState() {
        state = nextState[state];
        timer = 0;
    }

    void setDirection(bool direction) {

        sprite.flipX = direction;

        if (direction && Mathf.Sign(punchbox.localPosition.x) > 0)
        {
            punchbox.localPosition = new Vector3(-punchbox.localPosition.x, punchbox.localPosition.y, punchbox.localPosition.z);
        }
        else if (!direction && Mathf.Sign(punchbox.localPosition.x) < 0)
        {
            punchbox.localPosition = new Vector3(-punchbox.localPosition.x, punchbox.localPosition.y, punchbox.localPosition.z);
        }
    }
}
