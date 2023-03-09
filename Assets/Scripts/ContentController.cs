using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentController : MonoBehaviour
{
    public GameObject title;
    public List<GameObject> content = new();

    private void Start()
    {
        ShowTitle(false);
        ShowContent(false);
    }
    public void ShowContent(bool show)
    {
        content.ForEach(x => x.SetActive(show));
    }
    public void ShowTitle(bool show)
    {
        if (!title) return;
        title.SetActive(show);
    }
}