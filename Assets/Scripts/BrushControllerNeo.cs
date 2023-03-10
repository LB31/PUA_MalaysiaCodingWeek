using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushControllerNeo : MonoBehaviour
{
    public GameObject ImageToSwipe;
    public GameObject ImageToSwipeProp
    {
        get { return ImageToSwipe; }
        set { ImageToSwipe = value; Initialize(); }
    }

    public float radius = 2;
    public string targetTag = "Target";
    public Color StartColor = Color.grey;
    public int MaxVertices = 30;

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;
    private int maxVexBackup;

    private int removedVertices;

    void Start()
    {
        maxVexBackup = MaxVertices;
        Initialize();
    }

    public void Initialize()
    {
        mesh = ImageToSwipe.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = StartColor;
        }

        mesh.colors = colors;

        removedVertices = 0;
        MaxVertices = maxVexBackup;
    }

    void Update()
    {
        // click
        if (Input.GetMouseButtonDown(0))
        {
        }
        // release
        if (Input.GetMouseButtonUp(0))
        {
        }
        // hold
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hit.transform.CompareTag(targetTag)) return;

                ReactToSwiping(hit);
            }
        }
    }

    private void ReactToSwiping(RaycastHit hit)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = ImageToSwipe.transform.TransformPoint(vertices[i]);
            float dist = Vector3.SqrMagnitude(vertex - hit.point);
            float radiusSqr = radius * radius;
            if (dist < radiusSqr)
            {
                float alpha = Mathf.Min(colors[i].a, dist / radiusSqr);
                float previousA = colors[i].a;
                colors[i].a = alpha;
                if (colors[i].a < 0.01f && colors[i].a != previousA)
                {
                    removedVertices++;
                    if (removedVertices >= MaxVertices)
                    {
                        MaxVertices = int.MaxValue;
                        // Finished swiping
                        Debug.Log("NO SWIPE");
                        PlayMakerFSM.BroadcastEvent("SwipingEnded");
                    }
                }
            }
        }

        mesh.colors = colors;
    }

}
