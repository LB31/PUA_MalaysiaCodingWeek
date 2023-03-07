using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacer : MonoBehaviour
{
    [HideInInspector] public UnityEvent PlacementFinished;
    [SerializeField] private GameObject placementObject;

    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;

    private GameObject placementIndicator;
    // From where to shoot on the tracked planes
    private Vector2 startPointRay;
    // Current plane hits
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
        planeManager = GetComponent<ARPlaneManager>();

        startPointRay = new Vector2(Screen.width * 0.5f, Screen.height * 0.3f);
    }

    private void Update()
    {
        PlaneDragging();
    }

    private void FinishPlacement()
    {
        // Notify other scripts that placement is complete
        PlacementFinished.Invoke();
        TogglePlanes(false);

        Debug.Log("FinishPlacement");
    }

    public void TogglePlanes(bool activate, bool onlyManager = false)
    {
        planeManager.enabled = activate;

        if (onlyManager) return;

        foreach (ARPlane plane in planeManager.trackables)
        {
            //plane.GetComponent<MeshRenderer>().enabled = activate;
            plane.gameObject.SetActive(activate);
            //Destroy(plane.gameObject);
        }
    }

    private void PlaneDragging()
    {
        // Check if AR plane was hit
        if (raycastManager.Raycast(startPointRay, hits, TrackableType.PlaneWithinPolygon))
        {
            // Get hit position
            Pose hitPose = hits[0].pose;
            Vector3 pos = hitPose.position;

            // Create new placement object
            if (placementIndicator == null)
            {
                // Check if placement indicator was assigned
                if (!placementObject)
                {
                    // Create substitute object
                    placementObject = new GameObject("placementIndicator");
                }
                // Create actual placement indicator
                placementIndicator = Instantiate(placementObject, pos, hitPose.rotation *= Quaternion.Euler(0, 0, 0));
            }
            // move placementIndicator
            else
            {
                placementIndicator.transform.position = pos;
                placementIndicator.transform.rotation = hitPose.rotation;

                // Check if desired alignement e.g. ceiling, wall was hit
                //if (CheckPlaneAlignment() && !CheckIfObjectsAround(pos))
                //    PlaceObject();
            }
        }
    }

    private void PlaceObject()
    {

    }

}
