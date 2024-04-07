using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public AudioSource WalkSFX;

    public Animator animator;
    public bool FaceRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Move(Vector3 newpos) {
        transform.position = newpos;
        animator.SetBool ("Walk", true);
        if (!WalkSFX.isPlaying){
            WalkSFX.Play();
        }
        
    }

    public void Stop() {
        animator.SetBool ("Walk", false);
        WalkSFX.Stop();
    }

    public void turn() {
        FaceRight = !FaceRight;

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
