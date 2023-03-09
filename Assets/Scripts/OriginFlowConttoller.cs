using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OriginFlowConttoller : MonoBehaviour
{
    public List<Objects> content;
    public GameObject panel;
    public Button nextButton;
    public TextMeshProUGUI buttonText;
    private int actualStep = 0;

    private readonly string nextText = "Next";
    private readonly string restartText = "Restart";

    private void OnEnable()
    {
        nextButton.onClick.AddListener(ActivateContent);
    }
    private void OnDisable()
    {
        nextButton.onClick.RemoveAllListeners();
    }
    private void ActivateContent()
    {
        if (actualStep == 0)
            buttonText.text = nextText;

        //Deactivate current content
        foreach (GameObject go in content[actualStep].ContentObjects)
            go.SetActive(false);

        actualStep++;

        //Activate next content
        foreach (GameObject go in content[actualStep].ContentObjects)
            go.SetActive(true);

        if (actualStep + 1 >= content.Count)
            Restart();
    }
    private void Restart()
    {
        actualStep = 0;
        buttonText.text = restartText;
    }

    [Serializable]
    public struct Objects
    {
        public List<GameObject> UIObjects;
        public List<GameObject> ContentObjects;
    }
}
