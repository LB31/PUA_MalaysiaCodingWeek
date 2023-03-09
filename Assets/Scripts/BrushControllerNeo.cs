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

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    void Start()
    {
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
                colors[i].a = alpha;
            }          
        }

        mesh.colors = colors;
    }

}
