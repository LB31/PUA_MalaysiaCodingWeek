using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacer : MonoBehaviour
{
    [HideInInspector] public UnityEvent PlacementFinished;

    [SerializeField] private GameObject actualObject;
    [SerializeField] private GameObject placementIndicator;

    // Buttons
    [SerializeField] private GameObject buttonPlacement;
    [SerializeField] private GameObject buttonRestart;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;

    // From where to shoot on the tracked planes
    private Vector2 startPointRay;
    // Current plane hits
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool placed;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

        startPointRay = new Vector2(Screen.width * 0.5f, Screen.height * 0.3f);

        TogglePlacement(true);
    }

    private void Update()
    {
        if (placed) return;

        PlaneDragging();
    }

    private void PlaneDragging()
    {
        // Check if AR plane was hit
        if (raycastManager.Raycast(startPointRay, hits, TrackableType.PlaneWithinPolygon))
        {
            // Show indicator again
            placementIndicator.SetActive(true);

            // Get hit position
            Pose hitPose = hits[0].pose;

            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
        } else
            placementIndicator.SetActive(false);
    }

    public void PlaceObject()
    {
        FinishPlacement();
        TogglePlacement(false);

        actualObject.transform.SetPositionAndRotation(placementIndicator.transform.position, placementIndicator.transform.rotation);
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

    private void TogglePlacement(bool activatePlacement)
    {
        buttonPlacement.SetActive(activatePlacement);
        buttonRestart.SetActive(!activatePlacement);

        placementIndicator.SetActive(activatePlacement);
        actualObject.SetActive(!activatePlacement);

        placed = !activatePlacement;
    }

    public void RestartPlacing()
    {
        TogglePlacement(true);
        TogglePlanes(true);
    }



}
