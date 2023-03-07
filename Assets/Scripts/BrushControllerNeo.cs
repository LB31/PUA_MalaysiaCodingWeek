using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushControllerNeo : MonoBehaviour
{
    public Transform Brush;
    public GameObject ImageToSwipe;

    public Mesh mesh;
    public Vector3[] vertices;
    public Color[] colors;

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
            Debug.Log("click");
        }
        // release
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("release");
        }
        // hold
        if (Input.GetMouseButton(0))
        {
            Debug.Log("HOLD");

            Vector3 hitPoint = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log(hit.transform.name);

                Brush.SetPositionAndRotation(hit.point, hit.transform.rotation);

                ReactToSwiping();
            }
        }
    }

    private void ReactToSwiping()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i].a -= 0.1f * Time.deltaTime;
        }
        mesh.colors = colors;
    }
}
