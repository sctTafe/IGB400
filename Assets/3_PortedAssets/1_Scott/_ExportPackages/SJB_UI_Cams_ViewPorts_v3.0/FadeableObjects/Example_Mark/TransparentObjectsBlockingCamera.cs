using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author Mark Hoey
/// Description: This script is a simple waypoint movement system
/// </summary>
public class TransparentObjectsBlockingCamera : MonoBehaviour
{
    //Private variables but SerializeField allows them to show up in the Unity Editor
    [SerializeField] private GameObject lookObject;
    [SerializeField] private GameObject lookCamera;

    //Private variables 
    private FadeableObject[] hideableObjects = new FadeableObject[0];

    void Start()
    {
        //Search the scene and get all objects with the FadeableObject script attached to them
        hideableObjects = GameObject.FindObjectsOfType<FadeableObject>();
    }

    void Update()
    {
        DetectObjects();
    }

    //This method detects if objects are between the camera and the target object it should look at
    void DetectObjects()
    {
        //Calculate the vector between camera and target:
        Vector3 direction = lookObject.transform.position - lookCamera.transform.position;
        //If needed the below line could be added
        //direction.Normalize(); //Normalize vector

        //Get the distance between camera and target:
        float distance = direction.magnitude;

        //Create a ray from the camera to the target
        Ray ray = new Ray(lookCamera.transform.position, direction);
        //Draw a debug version of this ray in the editor (can be omitted)
        Debug.DrawRay(ray.origin, ray.direction * distance);

        //Detect all the objects along the ray that have a collider
        RaycastHit[] objectsAlongRay = Physics.RaycastAll(ray.origin, ray.direction, distance);

        //If there are objects between the target and the camera and there are objects with the FadeableObject script
        if (objectsAlongRay.Length > 0 && hideableObjects.Length > 0)
        {
            //Make all objects unfaded
            for (int i = 0; i < hideableObjects.Length; i++)
            {
                //Temporarily cache the object in the array
                GameObject tempObject = hideableObjects[i].transform.gameObject;

                //This would turn on the gameobject completely
                //tempObject.SetActive(false);

                //This is the basic way to change the alpha but it's very harsh
                //tempObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 1f));

                //This is the smoother way to fade it in
                tempObject.GetComponent<FadeableObject>().isFaded = false;


                //Make only the objects along the ray faded
                for (int j = 0; j < objectsAlongRay.Length; j++)
                {
                    //Temporarily cache the object in the array
                    GameObject tempRayObject = objectsAlongRay[j].transform.gameObject;

                    //If it's got a FadeableObject script and is not the actual target then do something to it
                    if (tempObject == tempRayObject && tempRayObject != lookObject)  
                    {    
                            //This would turn off the gameobject completely
                            //tempObject.SetActive(false);

                            //This is the basic way to change the alpha but it's very harsh
                            //tempObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 1, 1, 0.2f));

                            //This is the smoother way to fade it out
                            tempObject.GetComponent<FadeableObject>().isFaded = true;
                    }                 
                }              
            }         
        }
    }
}
