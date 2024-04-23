using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject nextFire;

    LineRenderer lineRenderer;

    Node [] fireNode;
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.08f;
        lineRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.positionCount = 2;
        lineRenderer.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, nextFire.transform.position);
    }
}
