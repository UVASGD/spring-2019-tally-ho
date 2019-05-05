using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SINGLETON : MonoBehaviour
{

    private static SINGLETON instance = null;

    private void Awake()
    {
        if(instance == null) {
            DontDestroyOnLoad(this);
            instance = this;
        } else {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
