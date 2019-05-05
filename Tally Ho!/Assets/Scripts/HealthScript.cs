using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour {
    [SerializeField]
    private int health = 100;

    private AudioSource hurtSound;
    private bool haveSound;

    // Start is called before the first frame update
    void Start() {
        if (GetComponent<AudioSource>() != null) {
            hurtSound = GetComponent<AudioSource>();
            haveSound = true;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D trigCollider) {
        if (trigCollider.gameObject.tag == "player attack" && !gameObject.CompareTag("Player")) {
            if (haveSound) {
                hurtSound.Play();
            }
            health -= 10;
            if (health == 0) {
                Destroy(gameObject);
            }
        }
        if (trigCollider.gameObject.tag == "Enemy Attack" && gameObject.CompareTag("Player")) {
            if (haveSound) {
                hurtSound.Play();
            }
            health -= 10;
            if (health == 0) {
                //Do Something
                SceneManager.LoadScene("DeathScreen");
                //Destroy(gameObject);
            }
        }
    }
}
