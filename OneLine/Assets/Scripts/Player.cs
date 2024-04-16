using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //public AudioSource WalkSFX;

    private Animator animator;

    /*
    public GameObject Char1; //basic line
    public GameObject Char2; //on fire
    public GameObject Char3; //on ice
    public Animator anim1;
    public Animator anim2;
    public Animator anim3;
    */

    public bool FaceRight = true;

    private Path pathFollower;

    public bool fire = true;

    // Start is called before the first frame update
    void Start()
    {
        /*
         Char1.SetActive(true);
         Char2.SetActive(false);
         Char3.SetActive(false);
         animator = anim1;
         */

        pathFollower = GameObject.FindGameObjectWithTag("Path").GetComponent<Path>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(pathFollower.stopRight + " " + pathFollower.stopLeft);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Wall") {
            if (FaceRight) {
                pathFollower.stopRight = true;
            } else {
                pathFollower.stopLeft = true;
            }
            Debug.Log("DID thing");
            Box wall = other.gameObject.GetComponent<Box>();
            if (fire && wall.isIce) {
                moveFree();
                Debug.Log("Player is on fire");
                other.gameObject.SetActive(false);
            
            }
        }
    }

    public void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Wall") {
            moveFree();
        }
    }

    public void moveFree() {
        pathFollower.stopRight = false;
        pathFollower.stopLeft = false;
        Debug.Log("Free");
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
        pathFollower.cap();
    }

    /*
    void SwitchCharacter(int charNum)
    {
        if (charNum == 1)
        {
            Char1.SetActive(true);
            Char2.SetActive(false);
            Char3.SetActive(false);
            animator = anim1;
        }
        else if (charNum == 2)
        {
            Char1.SetActive(false);
            Char2.SetActive(true);
            Char3.SetActive(false);
            animator = anim2;
        }
        else if (charNum == 3)
        {
            Char1.SetActive(false);
            Char2.SetActive(false);
            Char3.SetActive(true);
            animator = anim3;
        }
        else {
            Debug.Log("SwitchCharacter is being given a wrong number");
        }
    }
    */


}
