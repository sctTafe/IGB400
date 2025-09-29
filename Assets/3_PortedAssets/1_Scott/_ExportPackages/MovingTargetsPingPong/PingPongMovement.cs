using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{


    /// <summary>
    /// Author: Andre Milne-Jones
    /// Contributor: Scott Barley, Christopher Rudel
    /// Description: This script moves a floating platform.
    /// Based on:
    /// https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html
    /// </summary>

    public class PingPongMovement : MonoBehaviour
    {

        [SerializeField] bool horizontal = false;                                                               // Is the platform moving up or sideways
        [SerializeField] float distance = 0;                                                                    // The distance you want it to move
        [SerializeField] float breakTime = 2;                                                                   // How long you want the platform to wait for inbetween each movement
        [SerializeField] float speed = 1;
        Vector3 startPosition;                                                                                  // Holds the start position
        Vector3 temp;



        void Start()
        {
            startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            float flypp = Mathf.PingPong(speed * Time.time, distance + (speed * breakTime));                    // Allows for you to speed up or slow down how long it takes to travel the distance by timeMultiplyer, which effects time.time and the break times.
            float Pos1 = Mathf.Clamp(flypp, 0 + (speed * breakTime / 2), distance + (speed * breakTime / 2));   // Clamps the pingpong output to just the distance length, with half the break on either side
            float pos2 = Pos1 - speed * breakTime / 2;                                                          // Allows for speeding up or slowing down the time it takes to travel the distance by muliplying the half break buffers by the time muliplyer
            if (horizontal) { temp = new Vector3(0, 0, pos2); }                                                 // Temporary vector3, where only the value that will be added to the actual position is held                                                                                            
            else { temp = new Vector3(0, pos2, 0); }                                                            // Temporary vector3, where only the value that will be added to the actual position is held                                                                                           
            Vector3 truePos = temp + startPosition;                                                             // Creates a new vector 3, where it adds the previous vector three to its start position 
            transform.position = truePos;
        }
    }
}