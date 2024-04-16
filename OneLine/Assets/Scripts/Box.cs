using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Box : MonoBehaviour
{
    public bool isIce = false;

    private PathFollower pathFollower;
    // Start is called before the first frame update
    void Start()
    {
        pathFollower = GameObject.FindGameObjectWithTag("Path").GetComponent<PathFollower>();
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
