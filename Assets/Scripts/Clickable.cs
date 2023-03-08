using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour
{
    //speed of the object
    public float speed = 6.0f;

    //the target position
    public Transform target;

    //specify necessary object
    public Transform ball1;
    public Transform ball2;
    public Transform ball3;

    //declare respective original object's position
    private Vector3 originBall1;
    private Vector3 originBall2;
    private Vector3 originBall3;

    //declare original position of the selected object (front object)
    private Vector3 originPosition;

    private void Start()
    {
        //store respective original object's position
        originBall1 = ball1.position;
        originBall2 = ball2.position;
        originBall3 = ball3.position;
    }

    private void Update()
    {
        //mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            /* 
             Debug.Log("1: " + ball1.position + " " + originBall1);
             Debug.Log("2: " + ball2.position + " " + originBall2);
             Debug.Log("3: " + ball3.position + " " + originBall3);
            */

            //if none is at front, selected object will move to front
            if (ball1.position == originBall1 && ball2.position == originBall2 && ball3.position == originBall3)
            {
                Debug.Log("Nothing in front, move an object to front");

                //get the position of the mouse click and store in ray variable
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //end point of the ray
                RaycastHit hit;

                //runs if the ray hits
                if (Physics.Raycast(ray, out hit))
                {
                    //declare the variable to store the selected object
                    Transform objectPosition = hit.transform;

                    //Set the original position so that the object at front could be move back to that position in the future
                    originPosition = hit.transform.position;

                    Debug.Log(originPosition);

                    //starts the animation of the object to the front
                    StartCoroutine(moveObject(objectPosition));

                    //Set tags into 'Front' representing the object is at front
                    objectPosition.gameObject.tag = "Front";

                }
            }
            //if the object is animating, do nothing
            else if((ball1.position != originBall1 && ball2.position == originBall2 && ball3.position == originBall3 && ball1.position != target.position) ||
                (ball1.position == originBall1 && ball2.position != originBall2 && ball3.position == originBall3 && ball2.position != target.position) ||
                (ball1.position == originBall1 && ball2.position == originBall2 && ball3.position != originBall3 && ball3.position != target.position)
                )
            {
                Debug.Log("Object is moving, movement restricted.");
            }
            //if other object is selected, swap the object with the one at front
            else if((ball1.position != originBall1 && ball2.position == originBall2 && ball3.position == originBall3) ||
                (ball1.position == originBall1 && ball2.position != originBall2 && ball3.position == originBall3) ||
                (ball1.position == originBall1 && ball2.position == originBall2 && ball3.position != originBall3))
            {
                //swapping object
                Debug.Log("Ready to Swap Object");

                //get the position of the mouse click and store in ray
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //end point of the ray
                RaycastHit hit;
                //runs if the ray hits
                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectPosition = hit.transform;
                   
                    if(hit.collider.CompareTag("Front"))
                    {
                        Debug.Log("object already at front");
                    } else if(hit.collider.CompareTag("Waiting"))
                    {
                        Debug.Log("swapping");

                        //Get the object that is in front
                        GameObject front = GameObject.FindWithTag("Front");

                        Vector3 tempPosition = originPosition;

                        //Update the new original position
                        originPosition = hit.transform.position;

                        Debug.Log(originPosition);

                        //starts the animation of the swapping between the selected object and the front object
                        StartCoroutine(swapObject(objectPosition, front.transform, tempPosition));

                        //Set tags into 'Front' representing the object is at front
                        objectPosition.gameObject.tag = "Front";
                        front.tag = "Waiting";
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
    IEnumerator moveObject(Transform objectPosition)
    {
        //while the selected object is not at the front
        while (objectPosition.position != target.position)
        {
            var step = speed * Time.deltaTime; //calculate distance to move
            objectPosition.position = Vector3.MoveTowards(objectPosition.position, target.position, step);

            yield return null;

        }
    }

    //swap object with the one at front
    IEnumerator swapObject(Transform objectPosition, Transform frontPosition, Vector3 pos)
    {
        //while the selected object is not at the front nor the front object is not at its original position
        while (objectPosition.position != target.position || frontPosition.position != pos)
        {
            var step = speed * Time.deltaTime; //calculate distance to move

            //the object selected will move to the front
            objectPosition.position = Vector3.MoveTowards(objectPosition.position, target.position, step);

            //the front object will move back to its original position
            frontPosition.position = Vector3.MoveTowards(frontPosition.position, pos, step);

            yield return null;

        }
    }
}