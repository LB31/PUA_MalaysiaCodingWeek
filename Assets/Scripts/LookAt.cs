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
        Vector3 lookAtPosition = new Vector3(mainCameraTransform.position.x, transform.position.y, mainCameraTransform.position.z);
        transform.LookAt(lookAtPosition, Vector3.up);
    }
}