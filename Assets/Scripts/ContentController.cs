using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentController : MonoBehaviour
{
    public GameObject title;
    public List<GameObject> content = new();
    public bool destroy = false;
    public GameObject ContentPrefab;
    public bool IsActing;

    private void Start()
    {
        ShowTitle(false);
        ShowContent(false);
    }
    public void ShowContent(bool show)
    {
        if (destroy)
        {
            if (show)
                content.Add(Instantiate(ContentPrefab, transform));
            else
                content.ForEach(x => Destroy(x));
            return;
        }

        content.ForEach(x => x.SetActive(show));
    }
    public void ShowTitle(bool show)
    {
        if (!title) return;
        title.SetActive(show);
    }
}
