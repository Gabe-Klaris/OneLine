using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    Node [] PathNode;
    public GameObject player;

    public GameObject levelCompleteMenuUI;
    public float MoveSpeed;
    LineRenderer lineRenderer;

    int CurrentNode = 0;

    float pos;

    static Vector3 CurrentPositionHolder;

    public bool stopRight = false;

    public bool stopLeft = false;

    public Color defaultColor = Color.black;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    float previous;

    float moveRadius = 5;

    float minRadius = 0.94f;

    Player playerScript;

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
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

        /* SetupLineColors(); */

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

/*     void SetupLineColors()
    {
        List<GradientColorKey> colorKeys = new List<GradientColorKey>();
        List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>();
        float alpha = 1.0f;

        for (int i = 0; i < PathNode.Length - 1; i++) {
            Color currentColor = defaultColor;
            if (PathNode[i].tag == "Fire Node" && PathNode[i + 1].tag == "Fire Node") {
                Debug.Log("found fire");
                currentColor = c2;
            }
            colorKeys.Add(new GradientColorKey(currentColor, 0.0f));
            alphaKeys.Add(new GradientAlphaKey(alpha, 0.0f));
        }

        Gradient gradient = new Gradient();
        gradient.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
        lineRenderer.colorGradient = gradient;
    } */

    // sets start and end pos for linear transform
    public void CheckNode() {
        if (CurrentNode < PathNode.Length - 1 && CurrentNode >= 0) {
            Debug.Log("Current Node: " + CurrentNode);
            pos = 0;
            CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
            startPosition = PathNode[CurrentNode].transform.position;

            // rotation
            Quaternion rotation = Quaternion.LookRotation(CurrentPositionHolder - player.transform.position, transform.TransformDirection(Vector3.up));
            player.transform.rotation = new Quaternion( 0 , 0 , rotation.z , rotation.w );

        }
    }

    public void updateall() {
        for (int i = 0; i < PathNode.Length; i++) {
            Node node = PathNode[i].GetComponent<Node>();
            node.simulateDrag();
        }
    }

    // function for dragging nodes
    // if node is dragged within radius of prev and post node, it moves to that position
    // if node is dragged outside radius of prev and post node, it moves to the edge of the radius
    public Vector3 checkDrag(Node node) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        player.transform.position = Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos);
        
        int index = 0;
        for (int i = 0; i < PathNode.Length; i++) {
            Node node1 = PathNode[i].GetComponent<Node>();
            if (node == node1) {
                index = i;
                i = PathNode.Length;
            }
        }


        if (index == CurrentNode || index == CurrentNode + 1) {
            Debug.Log("dont happen");
            movePlayer();
        }
        Debug.Log(index);

        Vector3 prevNode = new Vector3(0, 0, 0);
        if (index == 0 || index == PathNode.Length - 1) {
            if (index == 0) {
                prevNode = PathNode[index + 1].transform.position;
            }
            else if (index == PathNode.Length -1) {
                prevNode = PathNode[index - 1].transform.position;
            }
            float dist = Vector3.Distance(mousePos, prevNode);
            Vector3 vdist = mousePos - prevNode;
            if (dist < moveRadius && dist > minRadius) {
                return mousePos;
            }
            else if (dist > moveRadius) {
                //Debug.Log(OnePointNormalize(vdist));
                //Debug.Log(prevNode);
                return prevNode + OnePointNormalize(vdist);
            }
            else if (dist < minRadius){
                return prevNode + OnePointMinNormalize(vdist);
            }
            else {
                Debug.Log("Error");
                return node.transform.position;
            
            }
        }
        else {
            Vector3 posNode = PathNode[index + 1].transform.position;
            prevNode = PathNode[index - 1].transform.position;

            float posDist = Vector3.Distance(mousePos, posNode);
            float prevDist = Vector3.Distance(mousePos, prevNode);
            if (prevDist < moveRadius && posDist < moveRadius && prevDist > minRadius && posDist > minRadius) {
                Debug.Log("This3");
                return mousePos;
            }

            else if (posDist > moveRadius && prevDist < moveRadius) {
                Debug.Log("This");
                Vector3 vdist = mousePos - posNode;
                return posNode + OnePointNormalize(vdist);
            }
            else if (prevDist > moveRadius && posDist < moveRadius) {
                Debug.Log("This2");
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            else if (prevDist < minRadius && posDist < minRadius) {
                Debug.Log("MinRadius!!!!!!!!!");
                return node.transform.position;
            }
            else if (posDist < minRadius) {
                Debug.Log("MinRadius");
                Vector3 vdist = mousePos - posNode;
                return posNode + OnePointMinNormalize(vdist);
            }
            else if (prevDist < minRadius) {
                Debug.Log("MinRadius");
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointMinNormalize(vdist);
            }
            else if (prevDist > posDist) {
                Debug.Log("not this");
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            else if (posDist > prevDist) {
                Debug.Log("not this");
                Vector3 vdist = mousePos - posNode;
                return posNode + OnePointNormalize(vdist);
            }

            else {
                Debug.Log("Error");
                return node.transform.position;
            }

        }
        
    }

    Vector3 OnePointNormalize(Vector3 dist) {
        Vector3 newPos = dist.normalized;
        return newPos * moveRadius;
    }

    Vector3 OnePointMinNormalize(Vector3 dist) {
        Vector3 newPos = dist.normalized;
        return newPos * minRadius;
    }

    void movePlayer() {
        CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
        //Debug.Log(CurrentNode);
        startPosition = PathNode[CurrentNode].transform.position;
        player.transform.position = Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos);

        Quaternion rotation = Quaternion.LookRotation(CurrentPositionHolder - player.transform.position, transform.TransformDirection(Vector3.up));
        player.transform.rotation = new Quaternion( 0 , 0 , rotation.z , rotation.w );
    }

    public void cap() {
        if (CurrentNode == 0 && pos <= 0) {
            player.transform.position = PathNode[0].transform.position;
        }
    }

    void backNode() {
        if (CurrentNode >= 0) {
            CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
            startPosition = PathNode[CurrentNode].transform.position;
            pos = Vector3.Distance(startPosition, CurrentPositionHolder);
            Debug.Log(pos);
            Quaternion rotation = Quaternion.LookRotation(startPosition - player.transform.position, transform.TransformDirection(Vector3.up));
            player.transform.rotation = new Quaternion( 0 , 0 , rotation.z , rotation.w );
        }
    }


    // Draw Line between nodes
    void DrawLine() {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var t = Time.time;
        for (int i = 0; i < PathNode.Length; i++)
        {
            lineRenderer.SetPosition(i, PathNode[i].transform.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(Vector3.Distance(PathNode[0].transform.position, PathNode[1].transform.position));
        //float move = Input.GetAxis("Horizontal");

        DrawLine();
        // go forward direction
        if ((Input.GetKey(KeyCode.RightArrow) || (Input.GetKey(KeyCode.D))) && !stopRight) {
            if (!playerScript.FaceRight) {
                playerScript.turn();
            }
            // pos is incrimented here
            pos += MoveSpeed;
            // checks if not at next node
            if (pos < Vector3.Distance(startPosition, CurrentPositionHolder)) {
                Debug.Log("Moving");
                // linear transform (goes to position pos between start and end position)
                playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                pos += MoveSpeed;
            } 
            else {
                // here means hit next node
                if (CurrentNode < PathNode.Length - 1 && CurrentNode >= 0) {
                    CurrentNode++;
                    CheckNode();
                    Debug.Log("next node");
                    pos += MoveSpeed;
                    playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                    pos += MoveSpeed;
                }
                else if (CurrentNode == PathNode.Length - 1) {
                    Debug.Log("hit end");
                    levelCompleteMenuUI.SetActive(true);
                    Time.timeScale = 0f;
                }
            }
            pos -= MoveSpeed;
        }

        // go back direction (same as above but in reverse direction)
        else if (Input.GetKey(KeyCode.LeftArrow) || (Input.GetKey(KeyCode.A)) && !stopLeft) {
            Debug.Log("FaceRight");
            Debug.Log(playerScript.FaceRight);
            if (playerScript.FaceRight) {
                playerScript.turn();
            }

            pos -= MoveSpeed;
            // checks if not hit previous node
            if (pos > 0) {
                Debug.Log("Moving");
                playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                pos -= MoveSpeed;
            } 
            else {
                if (CurrentNode > 0) {
                    CurrentNode--;
                    backNode();
                    pos -= MoveSpeed;
                    playerScript.Move(Vector3.MoveTowards(startPosition, CurrentPositionHolder, pos));
                    pos -= MoveSpeed;
                }
            }
            pos += MoveSpeed;
        }

        else {
            playerScript.Stop();
        }
    
    }
}
