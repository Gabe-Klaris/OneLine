using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricNode : MonoBehaviour
{
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            if (player.onFire || player.fire) {
                explosion.SetActive(true);
                StartCoroutine(Explode(1f));
            }
        }
    }

    IEnumerator Explode(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        explosion.SetActive(false);
    }
}
