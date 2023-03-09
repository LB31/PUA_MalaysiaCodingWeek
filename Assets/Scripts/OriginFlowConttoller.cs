using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OriginFlowConttoller : MonoBehaviour
{
    public GameObject[] content;
    public Button nextButton;
    private int actualStep = -1;

    private void OnEnable()
    {
        nextButton.gameObject.SetActive(true);
        nextButton.onClick.AddListener(ActivateContent);
    }
    private void ActivateContent()
    {
        if(actualStep != -1)
            content[actualStep].SetActive(false);

        actualStep++;
        content[actualStep].SetActive(true);
    }

}
