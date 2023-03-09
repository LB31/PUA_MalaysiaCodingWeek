using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyIslandController : MonoBehaviour
{
    public float Speed = 2;

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

    public void MoveObject(GameObject obj, Vector3 toLocalPos, float duration = 2)
    {
        StartCoroutine(MoveLocalCoroutine(obj, toLocalPos));
    }

    private IEnumerator MoveLocalCoroutine(GameObject obj, Vector3 toLocalPos, float duration = 2)
    {
        Vector3 originPos = obj.transform.position;
        float currentTime = 0;

        while (currentTime < duration)   // while still scaling
        {
            currentTime += Time.deltaTime;  // increment the current time

            float t = currentTime / duration;  // calculate the current progress (0-1)

            // set the object's new scale based on the current progress
            obj.transform.localPosition = Vector3.Lerp(originPos, toLocalPos, t);

            yield return null;  // wait for the next frame
        }
    }
}
