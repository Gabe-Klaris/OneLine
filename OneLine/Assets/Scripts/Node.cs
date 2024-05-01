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
        path = this.gameObject.transform.parent.gameObject;
        pathFuns = path.GetComponent<Path>();
        
    }

    public void changeline() {
        Debug.Log("ping");
    }

    public void ping() {
        Debug.Log("ping");
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
