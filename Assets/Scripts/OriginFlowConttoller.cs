using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OriginFlowConttoller : MonoBehaviour
{
    public GameObject[] content;
    public GameObject panel;
    public Button nextButton;
    public TextMeshProUGUI buttonText;
    private int actualStep = -1;

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
        //activate button
        if (nextButton.gameObject.activeSelf)
            nextButton.gameObject.SetActive(true);

        //Panel
        if (!panel.activeSelf)
            panel.SetActive(true);

        //Avoid errors on the first step
        if (actualStep != -1)
        {
            buttonText.text = nextText;
            content[actualStep].SetActive(false);
        }

        actualStep++;

        //Activate next content
        content[actualStep].SetActive(true);

        if (actualStep + 1 >= content.Length)
            Restart();
    }
    private void Restart()
    {
        actualStep = -1;
        buttonText.text = restartText;
    }
}
