using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public GameObject door;

    GameObject on;
    GameObject off;


    // Start is called before the first frame update
    void Start()
    {
        on = door.transform.GetChild(0).gameObject;
        off = door.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit() {
        door.SetActive(false);
        on.SetActive(true);
        off.SetActive(false);

    }

}
