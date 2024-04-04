using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Renderer rend;

    private Vector3 mousePosition;

    private GameObject path;

    private Path pathFuns;

    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        path = GameObject.FindGameObjectWithTag("Path");
        pathFuns = path.GetComponent<Path>();
        
    }

    void OnMouseOver() {
        rend.enabled = true;
    }

    void OnMouseExit() {
        rend.enabled = false;
        pathFuns.updateall();
    }


    void OnMouseDrag() {
        transform.position = pathFuns.checkDrag(this); 
    }

    public void simulateDrag() {
        Vector3 pos = transform.position;
        pos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
