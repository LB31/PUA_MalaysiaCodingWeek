using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyIslandController : MonoBehaviour
{
    public void ScaleObject(Transform objToScale, float finalScale, float duration = 2)
    {
        StartCoroutine(ScaleCoroutine(objToScale, finalScale, duration));
    }

    private IEnumerator ScaleCoroutine(Transform objToScale, float finalScale, float duration = 2)
    {
        Vector3 originalScale = objToScale.localScale;
        float currentTime = 0;

        while (currentTime < duration)   // while still scaling
        {
            currentTime += Time.deltaTime;  // increment the current time

            float t = currentTime / duration;  // calculate the current progress (0-1)

            // set the object's new scale based on the current progress
            objToScale.localScale = Vector3.Lerp(originalScale, Vector3.one * finalScale, t);

            yield return null;  // wait for the next frame

        }
    }
}
