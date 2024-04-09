using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public AudioSource WalkSFX;

    public Animator animator;
    public bool FaceRight = true;

    private Path pathFollower;
    // Start is called before the first frame update
    void Start()
    {
        pathFollower = GameObject.FindGameObjectWithTag("Path").GetComponent<Path>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Wall") {
            if (FaceRight) {
                pathFollower.stopRight = true;
            } else {
                pathFollower.stopLeft = true;
            }
            Debug.Log("DID thing");
        }
    }

    void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Wall") {
            pathFollower.stopRight = false;
            pathFollower.stopLeft = false;
        }
    }

    public void Move(Vector3 newpos) {
        transform.position = newpos;
        animator.SetBool("Walk", true);
        /* if (!WalkSFX.isPlaying){
            WalkSFX.Play();
        } */
        
    }

    public void Stop() {
        animator.SetBool("Walk", false);
        //WalkSFX.Stop();
    }


    public void turn() {
        FaceRight = !FaceRight;
        Debug.Log("Turn");
		// Multiply player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
