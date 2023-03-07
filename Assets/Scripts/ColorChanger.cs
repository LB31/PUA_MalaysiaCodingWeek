using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour
{

    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private List<ColorButton> colorButtons = new();

    private void Start()
    {
        foreach (ColorButton button in colorButtons)
            button.Button.onClick.AddListener(() => ChangeColor(button.Color));
    }

    private void ChangeColor(Color col)
    {
        mesh.material.color = col;
    }

    private void OnDestroy()
    {
        foreach (ColorButton button in colorButtons)
        {
            if (button.Button)
                button.Button.onClick.RemoveAllListeners();
        }
    }
}
[Serializable]
public struct ColorButton
{
    public Color Color;
    public Button Button;
}
