using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetTrigger("Punching");
        }

        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            anim.SetTrigger("Kicking");
        }

    }
}
