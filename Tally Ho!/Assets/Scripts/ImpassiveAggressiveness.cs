using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassiveAggressiveness : MonoBehaviour
{
    private bool walkingRight;
    private Rigidbody2D rb;
    public float force = 1;
    public GameObject[] hitboxes;
    public GameObject player;
    public float attackRange;
    public Sprite passive;
    public Sprite shouting;

    private SpriteRenderer spriteRenderer;

    private bool isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer.sprite == null)
            spriteRenderer.sprite = passive;

        walkingRight = true;
        rb = GetComponent<Rigidbody2D>();
        isAttacking = false;
        if (!player)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce((walkingRight ? 1 : -1) * new Vector2(force, 0));
        if (!isAttacking && (player.gameObject.transform.position - gameObject.transform.position).magnitude < attackRange)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.otherCollider.CompareTag("Ground"))
        {
            walkingRight = !walkingRight;
        }
    }

    public IEnumerator Attack()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.sprite = shouting;

        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.SetActive(true);
        }

        Debug.Log("hit!");

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = passive;

        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.SetActive(false);
        }

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = shouting;

        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.SetActive(true);
        }

        Debug.Log("hit!");

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = passive;

        foreach (GameObject hitbox in hitboxes)
        {
            hitbox.SetActive(false);
        }

        isAttacking = false;
    }
}
