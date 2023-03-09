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
    PlantData activePlant;

    private void Start()
    {
        activePlant = null;
    }

    private void FixedUpdate()
    {
        if (!cameraTrans)
        {
            if (Camera.main)
                cameraTrans = Camera.main.transform;
            else
                return;
        }

        Debug.DrawRay(cameraTrans.position, cameraTrans.forward * 10, Color.green);

        if (Physics.Raycast(cameraTrans.position, cameraTrans.forward, out RaycastHit hit, 10000))
        {
            if (activePlant!= null)
            {
                if (activePlant.Plant == hit.transform.gameObject) return;
            }
            //duration += Time.fixedDeltaTime;

            //if (duration >= secondsUntilShowingUI)
            {
                activePlant = plantsData.FirstOrDefault(x => x.Plant == hit.transform.gameObject);
                ActivatePlantChanges(true);
            }

            return;
        }

        //duration = 0;
        ActivatePlantChanges(false);
    }
    private void ActivatePlantChanges(bool activate)
    {
        if (activePlant == null) return;
        if (activePlant.PlantUI == null) return;

        activePlant.PlantUI.SetActive(activate);
        mesh.material.color = activate ? activePlant.Color : Color.black;
    }
}
[Serializable]
public class PlantData
{
    public Color Color;
    public GameObject Plant;
    public GameObject PlantUI;

    public PlantData(Color color, GameObject plantUI, GameObject plant)
    {
        Color = color;
        Plant = plant;
        PlantUI = plantUI;
    }
}
