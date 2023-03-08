using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushControllerNeo : MonoBehaviour
{
    public Transform Brush;
    public GameObject ImageToSwipe;
    public float radius = 2;
    public string targetTag = "Target";

    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;

    void Start()
    {
        mesh = ImageToSwipe.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
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

                Brush.SetPositionAndRotation(hit.point, hit.transform.rotation);

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
