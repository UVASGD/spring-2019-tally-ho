using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D trigCollider)
    {
        if (trigCollider.gameObject.tag == "player attack")
        {
            health -= 10;
        }
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}
