using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBetweenRooms : MonoBehaviour
{
    private float roomWidth, roomHeight;
    public float lerpSpeed;
    private GameObject target;//will be set to player
    // Start is called before the first frame update
    void Start()
    {
        roomWidth = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().roomWidth;
        roomHeight = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>().roomHeight;
        target = GameObject.Find("Cornelius");
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        int gridx = Mathf.RoundToInt(target.transform.position.x / roomWidth);
        int gridy = Mathf.RoundToInt(target.transform.position.y / roomHeight);
        float newx = gridx * roomWidth;
        float newy = gridy * roomHeight;
        Vector3 targetPos = new Vector3(newx, newy, transform.position.z);
        transform.position += (targetPos - transform.position) * lerpSpeed;
    }
}
