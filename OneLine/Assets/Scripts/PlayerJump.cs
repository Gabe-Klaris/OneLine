using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJump : MonoBehaviour
{

    public bool isGrounded = true;
    private Rigidbody2D myRb;

    public bool vertical;

    Box wall;

    public bool left;

    public bool right;

    public bool down;

    public GameObject overlord;

    GameObject path;
    Path pathscript;

    Path otherpath;

    Player otherplayer;
    
    public Player playerscript;

    private Animator animator;

    CameraFollow2DLERP mainCamera;

    bool prevright;

    bool prevleft;

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
            animatorif();
            animator.SetBool("Walk", false);
            spritemask.SetActive(false);
            vertical = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
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
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z > 0.382 && transform.rotation.z < 0.707) {
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(-5, 0), ForceMode2D.Impulse);
            Debug.Log("Jump left");
            myRb.gravityScale = 0;
            animatorif();
            animator.SetBool("Walk", false);
            left = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
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
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
        else if (playerscript.active && isGrounded && Input.GetKeyDown(KeyCode.Space) && transform.rotation.z < -0.382 && transform.rotation.z > -0.707) {
            isGrounded = false;
            myRb.isKinematic = false;  
            myRb.AddForce(new Vector2(5, 0), ForceMode2D.Impulse);
            myRb.gravityScale = 0;
            animatorif();
            Debug.Log("Jump right");
            animator.SetBool("Walk", false);
            right = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
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
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
        else if (playerscript.active && isGrounded && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && transform.rotation.z > -0.382 && transform.rotation.z < 0.382) {
            isGrounded = false;
            myRb.isKinematic = false;
            animatorif();
            animator.SetBool("Walk", false);
            spritemask.SetActive(false);
            down = true;
            prevleft = pathscript.stopLeft;
            prevright = pathscript.stopRight;
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
            pathscript.stopLeft = prevleft;
            pathscript.stopRight = prevright;
        }
    }

    void animatorif() {
        if (playerscript.fire || playerscript.onFire) {
            playerscript.fireAnim.SetTrigger("Jump");
        }
        else if (playerscript.ice || playerscript.onIce) {
            playerscript.iceAnim.SetTrigger("Jump");
        }
        else if (playerscript.electric || playerscript.onElectric) {
            playerscript.electricAnim.SetTrigger("Jump");
        }
        else {
            playerscript.animator.SetTrigger("Jump");
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
        if ((other.gameObject.tag == "Node" || other.gameObject.tag == "Fire Node") && other.gameObject.transform.parent.gameObject != path && playerscript.active && !isGrounded) {
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
            otherplayer.fireTimer = playerscript.fireTimer;
            otherplayer.iceTimer = playerscript.iceTimer;
            otherplayer.electricTimer = playerscript.electricTimer;
            playerscript.fire = false;
            playerscript.ice = false;
            playerscript.electric = false;

            if (playerscript.onFire) {
                otherplayer.fire = true;
                otherplayer.fireTimer = 2;
                playerscript.onFire = false;
            }
            else if (playerscript.onIce) {
                otherplayer.ice = true;
                otherplayer.iceTimer = 2;
                playerscript.onIce = false;
            }
            else if (playerscript.onElectric) {
                otherplayer.electric = true;
                otherplayer.electricTimer = 2;
                playerscript.onElectric = false;
            }

            node.ping();
        }
        if (other.gameObject.tag == "Wall") {
            if (down) {
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
            else {
                myRb.velocity = Vector2.zero;
                Debug.Log("Wall");
            }
            wall = other.gameObject.GetComponent<Box>();
            if (wall.isIce && (playerscript.onFire || playerscript.fire)) {
                other.gameObject.SetActive(false);
            }

            if (wall.isFire && (playerscript.onIce || playerscript.ice)) {
                other.gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("Wall");
    }


}
