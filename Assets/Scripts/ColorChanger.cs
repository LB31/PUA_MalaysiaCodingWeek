using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{
    [HideInInspector] public bool islandIsActive = true;

    [Header("Plants")]
    [SerializeField] private LayerMask plantLayer;
    [SerializeField] private float secondsUntilShowingUI = 1;
    [SerializeField] private List<PlantData> plantsData = new();

    [Header("Others")]
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float radius = 1;

    float duration = 0;
    PlantData activePlant = new();

    private void FixedUpdate()
    {
        if (Physics.SphereCast(cameraTrans.position, radius, cameraTrans.forward, out RaycastHit hit, 10, plantLayer))
        {
            duration += Time.fixedDeltaTime;

            if (duration >= secondsUntilShowingUI)
            {
                activePlant = plantsData.FirstOrDefault(x => x.PlantUI.transform.parent.transform == hit.transform);
                ActivatePlantChanges(true);
            }

            return;
        }

        duration = 0;
        ActivatePlantChanges(false);
    }
    private void ActivatePlantChanges(bool activate)
    {
        if (!activePlant.PlantUI) return;

        activePlant.PlantUI.SetActive(activate);

        if (activate)
            mesh.material.color = activePlant.Color;
    }
}
[Serializable]
public struct PlantData
{
    public Color Color;
    public GameObject PlantUI;
}
