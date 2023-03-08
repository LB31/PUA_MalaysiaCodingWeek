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

    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float radius = 1;
    [SerializeField] private float secondsUntilShowingUI = 1;
    [SerializeField] private LayerMask plantLayer;
    [SerializeField] private List<PlantData> plantsData = new();

    [Header("old")]
    [SerializeField] private MeshRenderer mesh;

    float duration = 0;
    PlantData activePlant = new();

    private void Start()
    {
        foreach (PlantData data in plantsData)
            data.Button.onClick.AddListener(() => mesh.material.color = data.Color);
    }

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

        mesh.material.color = activate ? activePlant.Color : Color.black;
    }

    private void OnDestroy()
    {
        foreach (PlantData data in plantsData)
        {
            if (data.Button)
                data.Button.onClick.RemoveAllListeners();
        }
    }
}
[Serializable]
public struct PlantData
{
    public Color Color;
    public Button Button;
    public GameObject PlantUI;
}
