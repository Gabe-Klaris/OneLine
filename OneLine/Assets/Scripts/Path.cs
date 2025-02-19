using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// path script
// but on game object with all nodes as children
// should only have nodes as immediate children
// nodes should be in order you want to move in
public class Path : MonoBehaviour
{
    /* The array of nodes the line is drawn across */
    private Node[] PathNode;

    /* The player object's script */
    Player playerScript;

    /* The current number node the player is at in the array */
    int CurrentNode = 0;

    /* The Player */
    public GameObject player;

    public GameObject levelCompleteMenuUI;
    public float MoveSpeed;
    LineRenderer lineRenderer;

    public bool selecting = false;
    int movingNode;

    bool down = false;

    // position between nodes
    float pos;

    // node moving from
    Vector3 startPosition;
    // node moving towards
    private Vector3 currentPosition;

    // whether the player is stopped to the right
    public bool stopRight = false;
    // whether the player is stopped to the left
    public bool stopLeft = false;


    // line renderer colors (not used)
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    // previous position (not used)
    float previous;

    // max distance between nodes
    float moveRadius = 3;

    GameObject gameHandler;

    GameHandler gameHandlerScript;

    // min distance between nodes
    float minRadius = 0.64f;

    int lineSmoothness = 50;

    // Start is called before the first frame update
    void Start()
    {
        // gets components
        PathNode = GetComponentsInChildren<Node>();
        playerScript = player.GetComponent<Player>();

        player.transform.position = PathNode[0].transform.position;
        CheckNode();

        // Code for setting line renderer for line view (copied from Unity Documentation)
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.08f;
        lineRenderer.positionCount = PathNode.Length;
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        gameHandler = GameObject.FindGameObjectWithTag("GameController");
        gameHandlerScript = gameHandler.GetComponent<GameHandler>();
    }

    /* Called when moving forward (right) and hitting a new node */
    public void CheckNode() {
        /*
              !!!DO NOT CHANGE!!! 
        MOVEMENT WILL BREAK IF REMOVED 
        */
                  pos = 0;

        /* Checks if player is within node array */
        if (CurrentNode < PathNode.Length - 1 && CurrentNode >= 0) {
            /* CurrentPosition is node in front (right) */
            currentPosition = PathNode[CurrentNode + 1].transform.position;

            /* StartPosition is node behind (left) */
            startPosition = PathNode[CurrentNode].transform.position;

            /* If between two fire nodes, sets player on fire */
            if (PathNode[CurrentNode].tag == "Fire Node" && PathNode[CurrentNode + 1].tag == "Fire Node") {
                playerScript.ice = false;
                playerScript.onFire = true;
                playerScript.fire = false;
                if (playerScript.onElectric || playerScript.electric) {
                    playerScript.runExplode = true;
                }
            }

            /* If leaving fire line, starts fire timer */
            else if (PathNode[CurrentNode].tag == "Fire Node" && PathNode[CurrentNode + 1].tag != "Fire Node") {
                playerScript.ice = false;
                playerScript.onFire = false;
                playerScript.fire = true;
                playerScript.fireTimer = 2;
            }

            /* If between two ice nodes, sets player on ice */
            else if (PathNode[CurrentNode].tag == "Ice Node" && PathNode[CurrentNode + 1].tag == "Ice Node") {
                playerScript.fire = false;
                playerScript.onIce = true;
                playerScript.ice = false;

            }
            
            /* If leaving ice line, starts ice timer */
            else if (PathNode[CurrentNode].tag == "Ice Node" && PathNode[CurrentNode + 1].tag != "Ice Node") {
                playerScript.fire = false;
                playerScript.onIce = false;
                playerScript.ice = true;
                playerScript.iceTimer = 2;
            }

            /* Rotates character to be perpendicular to line */
            Vector3 facingAngle = currentPosition - player.transform.position;
            Quaternion rotation = Quaternion.LookRotation(facingAngle);

            Vector3 normalizedAngle = facingAngle.normalized;
            Vector3 rightDirection = Vector3.right;
            float dotRight = Vector3.Dot(normalizedAngle, rightDirection);

            if (rotation.z != 0 && dotRight >= 0) {
                player.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
            /* Flips player upside down if facing left while moving forwards */
            else if (rotation.z != 0 && dotRight < 0) {
                player.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                Quaternion currentRotation = player.transform.rotation;
                Quaternion rotation180 = new Quaternion(0, 0, 1, 0);
                Quaternion newRotation = currentRotation * rotation180;
                player.transform.rotation = newRotation;
            }
            /* Hard check to catch bugs */
            else if (rotation.z == 0 && rotation.x == 0) {
                player.transform.rotation = new Quaternion();
            }
        }
    }

    // called when moving back and hitting a new node
    void backNode() {
        // if within array
        if (CurrentNode >= 0) {
            // current position is node moving towards (forward)
            currentPosition = PathNode[CurrentNode + 1].transform.position;
            // start position is node moving from (back)
            startPosition = PathNode[CurrentNode].transform.position;
            // position is total distance between nodes (at next node)
            pos = Vector3.Distance(startPosition, currentPosition);
            // same as check node but in reverse
            if (PathNode[CurrentNode].tag == "Fire Node" && PathNode[CurrentNode + 1].tag == "Fire Node") {
                playerScript.ice = false;
                playerScript.onFire = true;
                playerScript.fire = false;
                if (playerScript.onElectric || playerScript.electric) {
                    playerScript.runExplode = true;
                }
            }
            else if (PathNode[CurrentNode].tag != "Fire Node" && PathNode[CurrentNode + 1].tag == "Fire Node") {
                playerScript.ice = false;
                playerScript.fire = true;
                playerScript.fireTimer = 2;
            }
            else if (PathNode[CurrentNode].tag == "Ice Node" && PathNode[CurrentNode + 1].tag == "Ice Node") {
                playerScript.fire = false;
                playerScript.onIce = true;
                playerScript.ice = false;

            }
            else if (PathNode[CurrentNode].tag != "Ice Node" && PathNode[CurrentNode + 1].tag == "Ice Node") {
                playerScript.fire = false;
                playerScript.ice = true;
                playerScript.iceTimer = 2;
            }
            else {
                playerScript.onFire = false;
            }

            /* Handles character rotation (CHARACTER SHOULD ONLY ROTATE AROUND Z AXIS) */
            Vector3 facingAngle = startPosition - player.transform.position;
            Quaternion rotation = Quaternion.LookRotation(facingAngle);

            Vector3 normalizedAngle = facingAngle.normalized;
            Vector3 leftDirection = Vector3.left;
            float dotLeft = Vector3.Dot(normalizedAngle, leftDirection);

            if (rotation.z != 0 && dotLeft >= 0) {
                player.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
            }
            else if (rotation.z != 0 && dotLeft < 0) {
                player.transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
                Quaternion currentRotation = player.transform.rotation;
                Quaternion rotation180 = new Quaternion(0, 0, 1, 0);
                Quaternion newRotation = currentRotation * rotation180;
                player.transform.rotation = newRotation;
            }
            /* Bug Checks */
            else if (rotation.z == 0 && rotation.y == 0) {
                player.transform.rotation = new Quaternion(0, 0, -0.49531f, 0.50465f);
            }
            else if (rotation.z == 0 && rotation.x == 0) {
                player.transform.rotation = new Quaternion();
            }
        }
    }

    // function for updating all nodes; used for cleaner line render
    public void updateall() {
        for (int i = 0; i < PathNode.Length; i++) {
            Node node = PathNode[i].GetComponent<Node>();
            node.simulateDrag();
        }
    }

    public Vector3 MoveStick(Node node) {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 stickPos = node.gameObject.transform.position + (new Vector3(h, v, 0) * 0.1f);
        return checkMovement(stickPos, node);
    }

    public Vector3 MoveMouse(Node node) {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        return checkMovement(MousePos, node);
    }

    // function for dragging nodes
    // if node is dragged within radius of prev and post node, it moves to that position
    // if node is dragged outside radius of prev and post node, it moves to the edge of the radius
    public Vector3 checkMovement(Vector3 newPos, Node node) {
        player.transform.position = Vector3.MoveTowards(startPosition, currentPosition, pos);
        // searches for node in array
        int index = 0;
        for (int i = 0; i < PathNode.Length; i++) {
            Node node1 = PathNode[i].GetComponent<Node>();
            if (node == node1) {
                index = i;
                i = PathNode.Length;
            }
        }

        // if node is one of the nodes player is moving between
        if (index == CurrentNode || index == CurrentNode + 1) {
            // if player is moving on a wall while moving line move away
            if (stopRight && index == CurrentNode + 1) {
                pos -= MoveSpeed;
            }
            else if (stopLeft && index == CurrentNode) {
                pos += MoveSpeed;
            }
            else if (stopRight && index == CurrentNode) {
                pos += MoveSpeed;
            }
            else if (stopLeft && index == CurrentNode + 1) {
                pos -= MoveSpeed;
            }
            movePlayer();
        }

        Vector3 prevNode = new Vector3(0, 0, 0);
        // if moving to first or last node
        if (index == 0 || index == PathNode.Length - 1) {
            if (index == 0) {
                prevNode = PathNode[index + 1].transform.position;
            }
            else if (index == PathNode.Length -1) {
                prevNode = PathNode[index - 1].transform.position;
            }

            // distance between mouse and node before dragged node
            float dist = Vector3.Distance(newPos, prevNode);
            Vector3 vdist = newPos - prevNode;

            // if in radius, move to mouse position (all good)
            if (dist < moveRadius && dist > minRadius) {
                return newPos;
            }
            // if outside radius, move to edge of radius
            else if (dist > moveRadius) {
                return prevNode + OnePointNormalize(vdist);
            }
            // if inside min radius push outside radius
            else if (dist < minRadius){
                return prevNode + OnePointMinNormalize(vdist);
            }
            // something went wrong
            else {
                return node.transform.position;
            }
        }

        // moving node between two nodes
        else {
            // node before and node after node being dragged
            Vector3 posNode = PathNode[index + 1].transform.position;
            prevNode = PathNode[index - 1].transform.position;

            // distance between mouse and nodes around it
            float posDist = Vector3.Distance(newPos, posNode);
            float prevDist = Vector3.Distance(newPos, prevNode);

            // if in radius of both nodes, move to mouse position (all good)
            if (prevDist < moveRadius && posDist < moveRadius && prevDist > minRadius && posDist > minRadius) {
                return newPos;
            }
            // if outside radius of next node, move to edge of radius of next node
            else if (posDist > moveRadius && prevDist < moveRadius) {
                Vector3 vdist = newPos - posNode;
                return posNode + OnePointNormalize(vdist);
            }
            // if outside radius of prev node, move to edge of radius of prev node
            else if (prevDist > moveRadius && posDist < moveRadius) {
                Vector3 vdist = newPos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            // if inside both min radius, do not move (error case)
            else if (prevDist < minRadius && posDist < minRadius) {
                return node.transform.position;
            }
            // if inside min radius of next node, push to edge of min radius of next node
            else if (posDist < minRadius) {
                Vector3 vdist = newPos - posNode;
                return posNode + OnePointMinNormalize(vdist);
            }
            // if inside min radius of prev node, push to edge of min radius of prev node
            else if (prevDist < minRadius) {
                Vector3 vdist = newPos - prevNode;
                return prevNode + OnePointMinNormalize(vdist);
            }

            /* 
            since all other cases have been checked, this is when outside of 
                                    both radii
            */

            // push to edge of radius of closest node
            else if (prevDist > posDist) {
                Vector3 vdist = newPos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            // push to edge of radius of closest node
            else if (posDist > prevDist) {
                Vector3 vdist = newPos - posNode;
                return posNode + OnePointNormalize(vdist);
            }
            else {
                return node.transform.position;
            }
        }
    }

    Vector3 OnePointNormalize(Vector3 dist) {
        Vector3 newPos = dist.normalized;
        return newPos * moveRadius;
    }

    Vector3 OnePointMinNormalize(Vector3 dist) 
    {
        Vector3 newPos = dist.normalized;
        return newPos * minRadius;
    }

    // moves player to pos between start and end position
    void movePlayer() 
    {
        currentPosition = PathNode[CurrentNode + 1].transform.position;
        startPosition = PathNode[CurrentNode].transform.position;
        player.transform.position = Vector3.MoveTowards(startPosition, currentPosition, pos);
    }

    // bug check for moing before first nodeS
    public void cap() 
    {
        if (CurrentNode == 0 && pos <= 0) {
            player.transform.position = PathNode[0].transform.position;
        }
    }

    // when player changes line, snaps player to node
    public void SnapPlayer(GameObject node) 
    {
        // finds node in array
        int index = 0;

        for (int i = 0; i < PathNode.Length; i++) {
            if (PathNode[i].transform.gameObject == node) {
                index = i;
            }
        }
        if (index <= PathNode.Length -2) {
            CurrentNode = index;

            // setting position
            CheckNode();

            // moving player
            Debug.Log("snap");
            playerScript.Move(Vector3.MoveTowards(startPosition, currentPosition, pos));

            // rotation
            CheckNode();
        }
        else {
            Debug.Log("last node");
            CurrentNode = index - 1;

            // setting position
            backNode();

            // moving player
            playerScript.Move(Vector3.MoveTowards(startPosition, currentPosition, pos));

            // rotation
            backNode();
        }
    }

    // Draws the line between nodes
    void DrawLine() 
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        /* lineRenderer.positionCount = (PathNode.Length - 1) * lineSmoothness + 1; */
        /* CreateCurve(); */

        var t = Time.time;
        for (int i = 0; i < PathNode.Length; i++)
        {
            lineRenderer.SetPosition(i, PathNode[i].transform.position);
        }
    }

    void CreateCurve()
    {
        Vector3[] points = new Vector3[lineRenderer.positionCount];
        int index = 0;

        for (int i = 0; i < PathNode.Length - 1; i++) {
            Vector3 p0 = PathNode[Mathf.Max(i - 1, 0)].transform.position;
            Vector3 p1 = PathNode[i].transform.position;
            Vector3 p2 = PathNode[i + 1].transform.position;
            Vector3 p3 = PathNode[Mathf.Min(i + 2, PathNode.Length - 1)].transform.position;

            for (int j = 0; j < lineSmoothness; j++) {
                float t = j / lineSmoothness;
                points[index++] = CalculatePoint(t, p0, p1, p2, p3);
            }
        }

        points[index] = PathNode[PathNode.Length - 1].transform.position;
        lineRenderer.SetPositions(points);
    }

    Vector3 CalculatePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float b0 = 0.5f * (-t3 + 2f * t2 - t);
        float b1 = 0.5f * (3f * t3 - 5f * t2 + 2f);
        float b2 = 0.5f * (-3f * t2 + 4f * t2 + t);
        float b3 = 0.5f * (t3 - t2);

        return (b0 * p0) + (b1 * p1) + (b2 * p2) + (b3 * p3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float move = Input.GetAxisRaw("Horizontal");
        DrawLine();

        // dashing
        float currentMoveSpeed = MoveSpeed;
        if (Input.GetButton("Dash")) {
            currentMoveSpeed *= 2;
        }

        // go forward direction
        if (move > 0 && !stopRight && !selecting) {
            // turn around (if facing left, turn)
            if (!playerScript.FaceRight) {
                playerScript.turn();
            }

            // pos is always incrimented
            pos += currentMoveSpeed;

            // checks if not at next node
            if (pos < Vector3.Distance(startPosition, currentPosition)) {
                // linear transform (goes to position pos between start and end position)
                playerScript.Move(Vector3.MoveTowards(startPosition, currentPosition, pos));

                // updates position
                pos += currentMoveSpeed;
            }
            else {
                // here means hit next node
                if (CurrentNode < PathNode.Length - 2 && CurrentNode >= 0) {
                    if (playerScript.active) {
                    }
                    CurrentNode++;
                    CheckNode();

                    // updates position
                    pos += currentMoveSpeed;

                    // moves a bit more
                    playerScript.Move(Vector3.MoveTowards(startPosition, currentPosition, pos));

                    // updates position
                    pos += currentMoveSpeed;
                }

                // hit finish node of level
                else if (CurrentNode == PathNode.Length - 2 && PathNode[CurrentNode + 1].tag == "Finish" && playerScript.active) {
                    Debug.Log(PathNode[CurrentNode].tag);
                    if (gameHandlerScript.nextLevelName == "END") {
                        stopRight = true;
                        stopLeft = true;
                        playerScript.Victory();
                    }
                    else {
                        levelCompleteMenuUI.SetActive(true);
                        Time.timeScale = 0f;
                    }
                }
            }

            // removing offset put on at start
            pos -= currentMoveSpeed;
        }

        // go back direction (same as above but in reverse direction)
        else if (move < 0 && !stopLeft && !selecting) {
            // turn around (if facing right, turn)
            if (playerScript.FaceRight) {
                playerScript.turn();
            }
            // same structure as above
            // excepts decrementing pos
            // calls back node instead of check node
            pos -= currentMoveSpeed;
            // checks if not hit previous node
            if (pos > 0) {
                playerScript.Move(Vector3.MoveTowards(startPosition, currentPosition, pos));
                pos -= currentMoveSpeed;
            } 
            else {
                if (CurrentNode > 0) {
                    if (playerScript.active) {
                        Debug.Log("back" + CurrentNode);
                    }
                    CurrentNode--;
                    backNode();
                    pos -= currentMoveSpeed;
                    playerScript.Move(Vector3.MoveTowards(startPosition, currentPosition, pos));
                    pos -= currentMoveSpeed;
                }
            }
            pos += currentMoveSpeed;
        }

        // if not moving, stop player
        else {
            playerScript.Stop();
        }

        float dpadMove = Input.GetAxisRaw("Dpad");

        if (dpadMove > 0.5 && !selecting && !down && playerScript.active) {
            selecting = true;
            movingNode = CurrentNode + 1;
            PathNode[movingNode].selected = true;
            PathNode[movingNode].rend.enabled = true;
            down = true;
            Debug.Log("start forward");
        }
        else if (dpadMove < -0.5 && !selecting && !down && playerScript.active) {
            selecting = true;
            movingNode = CurrentNode;
            PathNode[movingNode].selected = true;
            PathNode[movingNode].rend.enabled = true;
            down = true;
            Debug.Log("start back");
        }
        else if (dpadMove > 0.5 && selecting && !down && playerScript.active) {
            PathNode[movingNode].selected = false;
            PathNode[movingNode].rend.enabled = false;
            updateall();
            movingNode++;
            down = true;
            if (movingNode >= PathNode.Length) {
                movingNode = 0;
            }
            PathNode[movingNode].selected = true;
            PathNode[movingNode].rend.enabled = true;
            Debug.Log("forward");
        }
        else if (dpadMove < -0.5 && selecting && !down && playerScript.active) {
            PathNode[movingNode].selected = false;
            PathNode[movingNode].rend.enabled = false;
            updateall();
            movingNode--;
            down = true;
            if (movingNode < 0) {
                movingNode = PathNode.Length - 1;
            }
            PathNode[movingNode].selected = true;
            PathNode[movingNode].rend.enabled = true;
            Debug.Log("back");
        }
        else if (dpadMove == 0 && selecting) {
            down = false;
        }
    }
    
    void Update() {
        if (Input.GetButtonDown("Return")) {
            Debug.Log("return");
            selecting = false;
            PathNode[movingNode].selected = false;
            PathNode[movingNode].rend.enabled = false;
            updateall();
        }
    }
}
