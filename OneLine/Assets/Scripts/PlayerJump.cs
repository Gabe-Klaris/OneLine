using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJump : MonoBehaviour
{

    public bool isGrounded = true;
    private Rigidbody2D myRb;

    public bool vertical;

    public bool left;

    public bool right;

    public bool down;

    public GameObject overlord;

    GameObject path;
    Path pathscript;

    Path otherpath;

    Player otherplayer;
    
    Player playerscript;

    private Animator animator;

    Node node;

    private GameObject spritemask;
    
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        myRb.isKinematic = true;
        animator = overlord.GetComponentInChildren<Animator>();
        spritemask = transform.GetChild(0).gameObject;
        playerscript = overlord.GetComponent<Player>();
        path = playerscript.path;
        pathscript = path.GetComponent<Path>();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            isGrounded = false;
            myRb.isKinematic = false;
            myRb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Debug.Log("I'm jumping");
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            spritemask.SetActive(false);
            vertical = true;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        else if (transform.position.y - overlord.transform.position.y < 0f && !isGrounded && vertical) {
            isGrounded = true;
            myRb.velocity = Vector2.zero;
            myRb.isKinematic = true;    
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            vertical = false;
            spritemask.SetActive(true);
            Debug.Log("I'm grounded");
            pathscript.stopLeft = false;
            pathscript.stopRight = false;
        }
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > 0.382 && transform.rotation.z < 0.707) {
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
            myRb.gravityScale = 0;
            animator.SetBool("Walk", false);
            left = true;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        else if (transform.position.x - overlord.transform.position.x > 0 && !isGrounded && left) {
            isGrounded = true;
            myRb.isKinematic = true;  
            myRb.velocity = Vector2.zero;
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            left = false;
            Debug.Log("I'm grounded");
            pathscript.stopLeft = false;
            pathscript.stopRight = false;
        }
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z < -0.382 && transform.rotation.z > -0.707) {
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
            myRb.gravityScale = 0;
            animator.SetBool("Walk", false);
            right = true;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        else if (transform.position.x - overlord.transform.position.x < 0 && !isGrounded && right) {
            isGrounded = true;
            myRb.isKinematic = true;
            myRb.velocity = Vector2.zero;
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            right = false;
            Debug.Log("I'm grounded");
            pathscript.stopLeft = false;
            pathscript.stopRight = false;
        }
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.S) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            isGrounded = false;
            myRb.isKinematic = false;
            Debug.Log("I'm jumping");
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            spritemask.SetActive(false);
            down = true;
            pathscript.stopLeft = true;
            pathscript.stopRight = true;
        }
        else if (overlord.transform.position.y - transform.position.y > 20 && !isGrounded && down) {
            isGrounded = true;
            myRb.velocity = Vector2.zero;
            myRb.isKinematic = true;    
            transform.position = new Vector3(overlord.transform.position.x, overlord.transform.position.y);  
            down = false;
            spritemask.SetActive(true);
            Debug.Log("I'm grounded");
            pathscript.stopLeft = false;
            pathscript.stopRight = false;
        }
    }

    void FixedUpdate() {
        if (left) {
            Vector2 newv = new Vector2(myRb.velocity.x + 0.196f, 0);
            myRb.velocity = newv;
        }
        else if (right) {
            Vector2 newv = new Vector2(myRb.velocity.x - 0.196f, 0);
            myRb.velocity = newv;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Node" && other.gameObject.transform.parent.gameObject != path && playerscript.active) {
            Debug.Log(other.gameObject.transform.parent + " " + path);
            node = other.gameObject.GetComponent<Node>();
            otherpath = other.gameObject.transform.parent.GetComponent<Path>();
            otherplayer = otherpath.player.GetComponent<Player>();
            playerscript.disappear();
            otherplayer.appear();
            otherpath.SnapPlayer(other.gameObject);
            playerscript.active = false;
            otherplayer.active = true;
            node.ping();
        }
    }


}
