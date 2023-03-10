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
        if (actualStep == -1)
        {
            Activate(false, content[^1].ContentObjects);
            Activate(false, content[^1].UIObjects);
        }
        else
        {
            Activate(false, content[actualStep].ContentObjects);
            Activate(false, content[actualStep].UIObjects);

        }


        actualStep++;

        //Activate next content
        Activate(true, content[actualStep].ContentObjects);
        Activate(true, content[actualStep].UIObjects);

        if (actualStep + 1 >= content.Count)
            Restart();
    }
    private void Restart()
    {
        actualStep = -1;
        buttonText.text = restartText;
    }

    private void Activate(bool activate, List<GameObject> list)
    {
        foreach (GameObject go in list)
            go.SetActive(activate);
    }

    [Serializable]
    public struct Objects
    {
        public List<GameObject> UIObjects;
        public List<GameObject> ContentObjects;
    }
}
