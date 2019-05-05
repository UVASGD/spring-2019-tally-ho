using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NervousPacing : MonoBehaviour {
    private bool walkingRight;
    private Rigidbody2D rb;
    public float force = 1;
    public GameObject player;
    public float attackRange;

    public Transform punchbox;

    private bool isAttacking;

    Animator anim;
    // Start is called before the first frame update
    void Start() {
        walkingRight = true;
        rb = GetComponent<Rigidbody2D>();
        isAttacking = false;
        if(!player) {
            player = GameObject.FindWithTag("Player");
        }
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        rb.AddForce((walkingRight ? 1 : -1) * new Vector2(force, 0));
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("peck") && (player.gameObject.transform.position - gameObject.transform.position).magnitude < attackRange) {
            GetComponent<Animator>().SetTrigger("attack");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.otherCollider.CompareTag("Ground")) {
            walkingRight = !walkingRight;
            setDirection(!walkingRight);
        }
    }

    void setDirection(bool direction)
    {

        GetComponent<SpriteRenderer>().flipX = !direction;

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
