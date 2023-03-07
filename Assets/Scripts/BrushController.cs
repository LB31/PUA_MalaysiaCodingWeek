using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BrushController : MonoBehaviour
{

    public Transform brush;
    public float thicknessOfLine = 0.5f;
    [HideInInspector]
    public Color Color;
    public Transform PlaneToDraw;
    [HideInInspector]
    public Transform SwipeImage;

    public float radius = 2;

    private TrailRenderer trailRenderer;
    private float radiusSqr { get { return radius * radius; } }
    private Mesh mesh;
    private Vector3[] vertices;
    [HideInInspector]
    public Color[] colors;
    [HideInInspector]
    public bool SwipingAway = true;
    [HideInInspector]
    public int RemovedVertices;
    [HideInInspector]
    public int MaxVertices = 30;


    // retrace line
    private int LineCounter = -1;

    private void Start()
    {
        trailRenderer = brush.GetComponent<TrailRenderer>();

        trailRenderer.startWidth = thicknessOfLine;
        trailRenderer.endWidth = thicknessOfLine;

        trailRenderer.enabled = false;
    }

    public void InitializeSwiping()
    {
        trailRenderer.startColor = Color;
        trailRenderer.endColor = Color;

        // get information of swiping image
        mesh = SwipeImage.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
        }

        UpdateColor();
    }

    public void StopSwiping()
    {
        SwipeImage = null;
        PlaneToDraw = null;
    }

    void Update()
    {
        if (SwipeImage == null)
        {
            return;
        }

        Vector3 hitPoint = Vector3.zero;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.Equals(PlaneToDraw))
            {
                hitPoint = hit.point;
            }
        }
        // touching screen
        if (!SwipingAway && Input.GetMouseButtonDown(0))
        {
            trailRenderer.enabled = true;
        }
        // releasing screen
        if (!SwipingAway && Input.GetMouseButtonUp(0))
        {
            CreateLineObject();
            trailRenderer.Clear();
            trailRenderer.enabled = false;
        }

        // when moving finger and hitting something
        if (Input.GetMouseButton(0) && hit.transform != null)
        {
            if (hit.transform.Equals(PlaneToDraw))
            {
                brush.position = new Vector3(hitPoint.x, hitPoint.y, hitPoint.z);
                brush.rotation = hit.transform.rotation;
            }

            // swipe away
            if (SwipingAway && hit.transform.Equals(SwipeImage))
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    Vector3 vertex = SwipeImage.TransformPoint(vertices[i]);
                    float dist = Vector3.SqrMagnitude(vertex - hit.point);
                    if (dist < radiusSqr)
                    {
                        float alpha = Mathf.Min(colors[i].a, dist / radiusSqr);
                        float previousA = colors[i].a;
                        colors[i].a = alpha;
                        // previousA prevents from counting "removed" pixels twice
                        if (colors[i].a < 0.01f && colors[i].a != previousA)
                        {
                            RemovedVertices++;
                            if (RemovedVertices >= MaxVertices)
                            {
                                SwipingAway = false;
                                SwipeImage.gameObject.SetActive(false);
                                PlaneToDraw.gameObject.SetActive(true);
                            }
                        }

                    }

                }

                UpdateColor();
            }
        }

    }

    private void CreateLineObject()
    {
        LineCounter++;
        GameObject newLine = new GameObject("Line segment " + LineCounter);
        LineRenderer lr = newLine.AddComponent<LineRenderer>();

        RetraceLine(lr);
    }

    private void RetraceLine(LineRenderer line)
    {
        // Set the width of the Line Renderer
        line.startWidth = thicknessOfLine;
        line.endWidth = thicknessOfLine;
        // Set the number of vertices for the Line Renderer
        line.positionCount = trailRenderer.positionCount;

        line.material = trailRenderer.materials[0];

        Vector3[] rayPositions = new Vector3[trailRenderer.positionCount];
        trailRenderer.GetPositions(rayPositions);
        line.SetPositions(rayPositions);

        line.startColor = Color;
        line.endColor = Color;

        line.useWorldSpace = false;
        line.transform.parent = PlaneToDraw.parent;
    }

    private void UpdateColor()
    {
        mesh.colors = colors;
    }
}
