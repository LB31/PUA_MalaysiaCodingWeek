using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 targetPosition = new Vector3(mainCameraTransform.transform.position.x, transform.position.y, mainCameraTransform.transform.position.z);
        transform.LookAt(2 * transform.position - targetPosition);
    }
}