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
    public bool onFire = false;

    public bool onIce = false;
    public bool onElectric = false;

    public bool fire = false;
    
    public bool ice = false;

    public bool electric = false;
    
    Box wall;

    public GameObject path;
    private Path pathFollower;
    GameObject jumper;
    GameObject PlayerArt_Default;
    GameObject PlayerArt_Fire;
    GameObject PlayerArt_Ice;
    GameObject PlayerArt_Electric;

    public float fireTimer;

    public float iceTimer;
    public float electricTimer;

    public Renderer rend;

    public Renderer rend2;

    public Renderer rend3;

    public bool active;

    bool victory = false;

    float VictoryTimer;

    PlayerJump jump;



    // Start is called before the first frame update
    void Start()
    {
        /*
         Char1.SetActive(true);
         Char2.SetActive(false);
         Char3.SetActive(false);
         animator = anim1;
         */

        pathFollower = path.GetComponent<Path>();
        animator = gameObject.GetComponentInChildren<Animator>();

        jumper = this.gameObject.transform.GetChild(0).gameObject;
        PlayerArt_Default = jumper.transform.GetChild(1).gameObject;
        PlayerArt_Fire = jumper.transform.GetChild(2).gameObject;
        PlayerArt_Ice = jumper.transform.GetChild(3).gameObject;
        PlayerArt_Electric = jumper.transform.GetChild(4).gameObject;

        jump = jumper.GetComponent<PlayerJump>();

        rend = PlayerArt_Default.GetComponent<Renderer>();
        rend2 = PlayerArt_Fire.GetComponent<Renderer>();
        rend3 = jumper.GetComponentInChildren<Renderer>();
        
        if (!active) {
            disappear();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onFire || fire) {
            //Debug.Log("I'm on fire!");
            PlayerArt_Default.SetActive(false);
            PlayerArt_Fire.SetActive(true);
        }
        else if (onIce || ice) {
            Debug.Log("I'm on ice!");
            PlayerArt_Default.SetActive(false);
            PlayerArt_Ice.SetActive(true);
        }
        else if (onElectric || electric) {
            Debug.Log("I'm electric!");
            PlayerArt_Default.SetActive(false);
            PlayerArt_Electric.SetActive(true);
        }
        else {
            PlayerArt_Default.SetActive(true);
            PlayerArt_Fire.SetActive(false);
            PlayerArt_Ice.SetActive(false);
            PlayerArt_Electric.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (fire) {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0) {
                onFire = false;
                fire = false;
                //Death();
            }
        }
        if (ice) {
            iceTimer -= Time.deltaTime;
            Debug.Log("Ice Timer: " + iceTimer);
            if (iceTimer <= 0) {
                Debug.Log("No longer ice");
                onIce = false;
                ice = false;
            }
            
        }
        else if (electric) {
            electricTimer -= Time.deltaTime;
            if (electricTimer <= 0) {
                Debug.Log("No longer electric");
                electric = false;
            }
        }

        else if (victory) {
            VictoryTimer -= Time.deltaTime;
            Vector3 hello = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
            if (VictoryTimer <= 0) {
                victory = false;
                pathFollower.levelCompleteMenuUI.SetActive(true);
                Time.timeScale = 0f;
            }
            //Victory();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Switch" && electric) {
            Switch switchScript = other.gameObject.GetComponent<Switch>();
            switchScript.door.SetActive(false);
        }
        /* else if (other.gameObject.tag == "Electric Node") {
            electricTimer = 5;
            electric = true;
        } */
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Wall") {
            animator.SetTrigger("Collide");
            if (FaceRight) {
                pathFollower.stopRight = true;
            } else {
                pathFollower.stopLeft = true;
            }
            Debug.Log("hit wall");
            wall = other.gameObject.GetComponent<Box>();
            if (wall.isIce && (onFire || fire)) {
                other.gameObject.SetActive(false);
            }

            if (wall.isFire && (onIce || ice)) {
                other.gameObject.SetActive(false);
            }
        }
    }

    public void Victory() {
        StartCoroutine(wait());
        victory = true;
        VictoryTimer = 5;
    }

    IEnumerator wait() {
        yield return new WaitForSeconds(1);
    }

    public void appear() {
        rend.enabled = true;
        rend2.enabled = true;
        rend3.enabled = true;
    }

    public void disappear() {
        rend.enabled = false;
        rend2.enabled = false;
        rend3.enabled = false;
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
        if (jump.isGrounded) {
            animator.SetBool("Walk", true);
        }
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
