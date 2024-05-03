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

    CameraFollow2DLERP mainCamera;

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

        mainCamera = Camera.main.GetComponent<CameraFollow2DLERP>();
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
            Debug.Log("Jump left");
            myRb.gravityScale = 0;
            animator.SetTrigger("Jump");
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
            myRb.gravityScale = 1;
            Debug.Log("I'm grounded");
            pathscript.stopLeft = false;
            pathscript.stopRight = false;
        }
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z < -0.382 && transform.rotation.z > -0.707) {
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
            myRb.gravityScale = 0;
            animator.SetTrigger("Jump");
            Debug.Log("Jump right");
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
            myRb.gravityScale = 1;
            Debug.Log("I'm grounded");
            pathscript.stopLeft = false;
            pathscript.stopRight = false;
        }
        else if (playerscript.active && isGrounded && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            isGrounded = false;
            myRb.isKinematic = false;
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
        Debug.Log("hello");
        if (other.gameObject.tag == "Node" && other.gameObject.transform.parent.gameObject != path && playerscript.active) {
            Debug.Log(other.gameObject.transform.parent + " " + path);
            node = other.gameObject.GetComponent<Node>();
            otherpath = other.gameObject.transform.parent.GetComponent<Path>();
            otherplayer = otherpath.player.GetComponent<Player>();
            playerscript.disappear();
            mainCamera.SetTarget(otherplayer.gameObject);
            otherplayer.appear();
            otherpath.SnapPlayer(other.gameObject);
            playerscript.active = false;
            otherplayer.active = true;
            otherplayer.fire = playerscript.fire;
            otherplayer.ice = playerscript.ice;
            otherplayer.electric = playerscript.electric;
            playerscript.fire = false;
            playerscript.ice = false;
            playerscript.electric = false;

            node.ping();
        }
        if (other.gameObject.tag == "Wall") {
            myRb.velocity = Vector2.zero;
            Debug.Log("Wall");
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Wall");
    }


}
