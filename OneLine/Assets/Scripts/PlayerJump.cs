using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJump : MonoBehaviour
{

    public bool isGrounded = true;
    private Rigidbody2D myRb;

    public bool vertical;

    public bool left;

    public GameObject overlord;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myRb.isKinematic = true;
        animator = overlord.GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(isGrounded);
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            isGrounded = false;
            myRb.isKinematic = false;
            myRb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Debug.Log("I'm jumping");
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            vertical = true;
        }
        else if (transform.position.y - overlord.transform.position.y < 0.49f && !isGrounded && vertical) {
            isGrounded = true;
            myRb.velocity = Vector2.zero;
            myRb.isKinematic = true;    
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y + 0.49f);  
            vertical = false;
            Debug.Log("I'm grounded");
        }
        else if (isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > 0.382 && transform.rotation.z < 0.707) {
            myRb.isKinematic = false;
            myRb.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("Walk", false);
            left = true;
        }
        else if (transform.position.x - overlord.transform.position.x > 0 && !isGrounded && left) {
            isGrounded = true;
            myRb.velocity = Vector2.zero;
            myRb.isKinematic = true;
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            left = false;
            Debug.Log("I'm grounded");
        }
        Debug.Log(transform.rotation.z);



    }



}
