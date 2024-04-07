using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    Node [] PathNode;
    public GameObject player;
    public float MoveSpeed;

    int CurrentNode = 0;

    float pos;

    static Vector3 CurrentPositionHolder;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    float previous;

    float moveRadius = 2;

    Player playerScript;

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        PathNode = GetComponentsInChildren<Node>();
        CheckNode();
        player.transform.position = PathNode[0].transform.position;
        playerScript = player.GetComponent<Player>();

        // Code for setting line renderer for line view (copied from Unity Documentation)
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.08f;
        lineRenderer.positionCount = PathNode.Length;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    // sets start and end pos for linear transform
    public void CheckNode() {
        if (CurrentNode < PathNode.Length - 1) {
            previous = pos;
            pos = 0;
            CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
            startPosition = PathNode[CurrentNode].transform.position;

        }
    }

    public void updateall() {
        for (int i = 0; i < PathNode.Length; i++) {
            Node node = PathNode[i].GetComponent<Node>();
            node.simulateDrag();
        }
    }

    public Vector3 checkDrag(Node node) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
        
        int index = 0;
        for (int i = 0; i < PathNode.Length; i++) {
            Node node1 = PathNode[i].GetComponent<Node>();
            if (node == node1) {
                index = i;
                i = PathNode.Length;
            }
        }

        if (index == CurrentNode || index == CurrentNode + 1) {
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
            if (dist < moveRadius) {
                return mousePos;
            }
            else {
                Debug.Log(OnePointNormalize(vdist));
                Debug.Log(prevNode);
                return prevNode + OnePointNormalize(vdist);
            }
        }
        else {
            Vector3 posNode = PathNode[index + 1].transform.position;
            prevNode = PathNode[index - 1].transform.position;

            float posDist = Vector3.Distance(mousePos, posNode);
            float prevDist = Vector3.Distance(mousePos, prevNode);

            if (prevDist < moveRadius && posDist < moveRadius) {
                return mousePos;
            }
            else if (prevDist >= moveRadius && posDist < moveRadius) {
                Vector3 vdist = mousePos - prevNode;
                return prevNode + OnePointNormalize(vdist);
            }
            else if (prevDist < moveRadius && posDist >= moveRadius) {
                Vector3 vdist = mousePos - posNode;
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

    void movePlayer() {
        CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
        Debug.Log(CurrentNode);
        startPosition = PathNode[CurrentNode].transform.position;
        player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
    }

    void backNode() {
        if (CurrentNode >= 0) {
            pos = previous;
            CurrentPositionHolder = PathNode[CurrentNode + 1].transform.position;
            Debug.Log(CurrentNode);
            startPosition = PathNode[CurrentNode].transform.position;
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
        float move = Input.GetAxis("Horizontal");

        DrawLine();
        // go forward direction
        if (move > 0) {
            // pos is incrimented here
            pos += MoveSpeed;
            // checks if not at next node
            if (player.transform.position != CurrentPositionHolder) {
                Debug.Log("Moving");
                // linear transform (goes to position pos between start and end position)
                player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
            } 
            else {
                // here means hit next node
                if (CurrentNode < PathNode.Length - 1) {
                    CurrentNode++;
                    CheckNode();
                    Debug.Log("Movin1g");
                    pos += MoveSpeed;
                    player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
                    // comment line above and uncomment line when player animation is added
                    // playerScript.Move(player.transform.position);
                }
            }
        }

        // go back direction (same as above but in reverse direction)
        else if (move < 0) {
            pos -= MoveSpeed;
            if (player.transform.position != startPosition) {
                Debug.Log("Moving");
                // comment line above and uncomment line when player animation is added
                // playerScript.Move(player.transform.position);
                player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
            } 
            else {
                if (CurrentNode > 0) {
                    CurrentNode--;
                    backNode();
                    pos -= MoveSpeed;
                    player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
                }
            }
        }
        // uncomment line when player animation is added
        /* else if ((move < 0 && playerScript.FaceRight) || (move > 0 && playerScript.FaceRight) {
			playerScript.turn();
		}) */

        else {
            // uncomment line when player animation is added
            //playerScript.Stop();
        }
    
    }
}
