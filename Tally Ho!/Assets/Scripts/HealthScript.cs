using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private int health = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D trigCollider)
    {
        if (trigCollider.gameObject.tag == "player attack" && !gameObject.CompareTag("Player"))
        {
            health -= 10;
            if (health == 0)
            {
                Destroy(gameObject);
            }
        }
        if(trigCollider.gameObject.tag == "Enemy Attack" && gameObject.CompareTag("Player"))
        {
            health -= 10;
            if (health == 0)
            {
                //Do Something
                Destroy(gameObject);
            }
        }
    }
}
