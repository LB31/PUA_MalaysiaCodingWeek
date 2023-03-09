using System.Collections;
using System.Collections.Generic;
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
    public Vector3 scaleChanged;

    //declare original position of the selected object (front object)
    private Vector3 originPosition;

    private Transform currentFrontObject;

    private Camera cam;

    private bool moving;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // return when not clicking
        if (!Input.GetMouseButtonDown(0)) return;
        // return when object is moving
        if (moving) return;

        //get the position of the mouse click and store in ray
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //runs if the ray hits
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //declare the variable to store the selected object
            Transform hitObject = hit.transform;
            //Set the original position so that the object at front could be move back to that position in the future
            originPosition = hitObject.position;

            //if none is at front, selected object will move to front
            if (currentFrontObject == null)
            {
                if (!hit.transform.CompareTag("Waiting")) return;

                //starts the animation of the object to the front
                StartCoroutine(MoveObject(hitObject));

                //Show island title
                if (hit.transform.TryGetComponent(out ContentController controller))
                    controller.ShowTitle(true);

                //Set tags into 'Front' representing the object is at front
                hitObject.gameObject.tag = "Front";
            }

            else
            {
                //Open Scene
                if (hit.collider.CompareTag("Front"))
                {
                    StartCoroutine(ScaleObjectUp(hitObject));

                    //Show content on island
                    if (hit.transform.TryGetComponent(out ContentController controller))
                        controller.ShowContent(true);
                }
                else if (hit.collider.CompareTag("Waiting"))     //swappping object
                {
                    Debug.Log("swapping");

                    Vector3 tempPosition = originPosition;

                    //starts the animation of the swapping between the selected object and the front object
                    StartCoroutine(SwapObjects(hitObject, currentFrontObject, tempPosition));

                    //Set tags into 'Front' representing the object is at front
                    hitObject.gameObject.tag = "Front";
                    currentFrontObject.tag = "Waiting";

                    //Hide content on island
                    if (hit.transform.TryGetComponent(out ContentController controller))
                    {
                        controller.ShowTitle(false);
                        controller.ShowContent(false);
                    }
                }
            }

        }
    }

    //move object to front
    IEnumerator MoveObject(Transform objectToMove)
    {
        moving = true;
        //while the selected object is not at the front
        while (!SamePositionApproximately(objectToMove.position, target.position))
        {
            float step = speed * Time.deltaTime; //calculate distance to move
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, target.position, step);

            yield return null;

        }
        currentFrontObject = objectToMove;
        moving = false;
    }

    //swap object with the one at front
    IEnumerator SwapObjects(Transform hitObj, Transform frontObject, Vector3 returnPos)
    {
        moving = true;
        //while the selected object is not at the front nor the front object is not at its original position
        while (!SamePositionApproximately(hitObj.position, target.position) ||
            !SamePositionApproximately(frontObject.position, returnPos))
        {
            float step = speed * Time.deltaTime; //calculate distance to move

            //the object selected will move to the front
            hitObj.position = Vector3.MoveTowards(hitObj.position, target.position, step);

            //the front object will move back to its original position
            frontObject.position = Vector3.MoveTowards(frontObject.position, returnPos, step);
            StartCoroutine(ScaleObjectDown(frontObject));

            yield return null;

        }
        currentFrontObject = hitObj;
        moving = false;
    }

    private IEnumerator ScaleObjectUp(Transform obj)
    {
        while (obj.localScale.x < scaleChanged.x)
        {
            obj.localScale += Vector3.one * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ScaleObjectDown(Transform obj)
    {
        while (obj.localScale.x > 1)
        {
            obj.localScale -= Vector3.one * Time.deltaTime;

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