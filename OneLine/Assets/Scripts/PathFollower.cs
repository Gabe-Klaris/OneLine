using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    Node [] PathNode;
    public GameObject player;
    public float MoveSpeed;

    int CurrentNode;

    float pos;

    static Vector3 CurrentPositionHolder;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    float previous;

    Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        PathNode = GetComponentsInChildren<Node>();
        CheckNode();

        // Code for setting line renderer for line view (copied from Unity Documentation)
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.05f;
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
    void CheckNode() {
        if (CurrentNode < PathNode.Length) {
            // used for backtracking
            previous = pos;
            pos = 0;
            CurrentPositionHolder = PathNode[CurrentNode].transform.position;
            startPosition = player.transform.position;

        }
    }

    void backNode() {
        if (CurrentNode > 0) {
            pos = previous;
            CurrentPositionHolder = player.transform.position;
            Debug.Log(CurrentNode - 1);
            startPosition = PathNode[CurrentNode -1].transform.position;
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
        DrawLine();
        
        // go forward direction
        if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D))) {
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
                if (CurrentNode < PathNode.Length) {
                    CurrentNode++;
                    CheckNode();
                    Debug.Log("Movin1g");
                    pos += MoveSpeed;
                    player.transform.position = Vector3.Lerp(startPosition, CurrentPositionHolder, pos);
                }
            }
        }

        // go back direction (same as above but in reverse direction)
        if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A))) {
            pos -= MoveSpeed;
            if (player.transform.position != startPosition) {
                Debug.Log("Moving");
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
    
    }
}
