using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class IslandTransformer : MonoBehaviour
{
    //speed of the object
    public float speed = 6.0f;
    public float movementThreshold = 0.1f;

    //the target position
    public Transform target;

    //speficies distance moved and scaled when front object clicked
    public Vector3 distanceChanged;
    public Vector3 scaleChanged;

    //declare original position of the selected object (front object)
    private Vector3 originPosition;

    public Transform currentFrontObject;

    private Camera cam;

    private bool moving;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        //mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            if (moving) return;

            //get the position of the mouse click and store in ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            //if none is at front, selected object will move to front
            if (currentFrontObject == null)
            {
                //runs if the ray hits
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (!hit.transform.CompareTag("Waiting")) return;

                    //declare the variable to store the selected object
                    Transform objectPosition = hit.transform;

                    //Set the original position so that the object at front could be move back to that position in the future
                    originPosition = hit.transform.position;

                    //starts the animation of the object to the front
                    StartCoroutine(MoveObject(objectPosition));

                    //Set tags into 'Front' representing the object is at front
                    objectPosition.gameObject.tag = "Front";

                }
            }

            //if other object is selected, swap the object with the one at front
            else if (currentFrontObject != null)
            {
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Transform hitObject = hit.transform;

                    //Open Scene
                    if (hit.collider.CompareTag("Front"))
                    {
                        Vector3 finalDistance = hitObject.position + distanceChanged;

                        StartCoroutine(OpenObject(hitObject, finalDistance));

                    }
                    else if (hit.collider.CompareTag("Waiting"))     //swappping object
                    {
                        Debug.Log("swapping");

                        Vector3 tempPosition = originPosition;

                        //Update the new original position
                        originPosition = hit.transform.position;

                        //starts the animation of the swapping between the selected object and the front object
                        StartCoroutine(SwapObjects(hitObject, currentFrontObject, tempPosition));

                        //Set tags into 'Front' representing the object is at front
                        hitObject.gameObject.tag = "Front";
                        currentFrontObject.tag = "Waiting";
                    }
                }
                else
                {
                    Debug.Log("Hit missed");
                }
            }
            else
            {
                //invalid option as only at most one could be at front at a time
                Debug.Log("Nothing happens");
            }

        }

    }

    //move object to front
    IEnumerator MoveObject(Transform objectPosition)
    {
        moving = true;
        //while the selected object is not at the front
        while (!SamePositionApproximately(objectPosition.position, target.position))
        {
            float step = speed * Time.deltaTime; //calculate distance to move
            objectPosition.position = Vector3.MoveTowards(objectPosition.position, target.position, step);

            yield return null;

        }
        currentFrontObject = objectPosition;
        moving = false;
    }

    //swap object with the one at front
    IEnumerator SwapObjects(Transform hitObj, Transform frontPosition, Vector3 returnPos)
    {
        moving = true;
        //while the selected object is not at the front nor the front object is not at its original position
        while (!SamePositionApproximately(hitObj.position, target.position) ||
            !SamePositionApproximately(frontPosition.position, returnPos))
        {
            float step = speed * Time.deltaTime; //calculate distance to move

            //the object selected will move to the front
            hitObj.position = Vector3.MoveTowards(hitObj.position, target.position, step);

            //the front object will move back to its original position
            frontPosition.position = Vector3.MoveTowards(frontPosition.position, returnPos, step);

            yield return null;

        }
        currentFrontObject = hitObj;
        moving = false;
    }

    //open up the front object
    IEnumerator OpenObject(Transform objectPosition, Vector3 finalDistance)
    {
        while (/*objectPosition.position != finalDistance || */objectPosition.localScale.x < scaleChanged.x)
        {
            var step = speed * Time.deltaTime; //calculate distance to move

            //the object selected will move to the front
            //objectPosition.position = Vector3.MoveTowards(objectPosition.position, finalDistance, step);

            //the front object will move back to its original position
            objectPosition.localScale += objectPosition.localScale * 0.1f;

            yield return null;
        }
    }

    private bool SamePositionApproximately(Vector3 a, Vector3 b)
    {
        if (Vector3.Distance(a, b) < movementThreshold)
            return true;

        return false;
    }

}