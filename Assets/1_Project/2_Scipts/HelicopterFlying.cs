using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scott.Barley.v2
{


    /// <summary>
    /// Description: This script controls a basic helicopter (using Rigidbody)
    /// Based on Mark Hoey's Script 
    /// </summary>
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class HelicopterFlying : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] PhoneTiltInput _phoneTiltInput;

        [SerializeField] Transform meshToTilt;

        [Header("Movement")]

        [SerializeField] private float forwardSpeed = 6;
        [SerializeField] private float sidewardSpeed = 3;

        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] private float maxRotationSpeed = 6;
        [SerializeField] private float stoppingDrag = 2;
        [SerializeField] private float stoppingAngularDrag = 5;

        [Header("Height")]
        [SerializeField] private float verticalSpeed = 3;
        [SerializeField] private float minHeight = 0f;
        [SerializeField] private float maxHeight = 10;

        [Header("Visuals")]
        [SerializeField] float _meshRotationAngle = 20;

        Rigidbody rigidbody;


        private float pitch = 0f; //x axis
        private float strafe = 0; // y axis

        private float yaw = 0; //y axis
        private float roll = 0; 

        public float lift; //up/down
        public bool onCeiling;
        private Vector3 rotationOfMesh;
        public bool onFloor;


        void Start()
        {
            rigidbody = this.GetComponent<Rigidbody>();
            rigidbody.maxAngularVelocity = maxRotationSpeed;
        }


        void Update()
        {

        }

        private void FixedUpdate()
        {
            MovementInput();
        }

        private void MovementInput()
        {
            // Debug / TODO - Jocystick sentitivity, anything under ~5% movment dont recognise, lerp the rest between 5%-100%
            yaw = 0f; // Turning // yaw => rotation
            pitch = 0f; // Accelerating Foward/Back
            strafe = 0f; // Accelerating SideToSide

            Vector2 input_Move = _playerInput.actions["Move"].ReadValue<Vector2>();

            pitch = input_Move.y;
            strafe = input_Move.x;

            yaw = _phoneTiltInput.fn_GetNormalizedSteerAngle(); // rotation

            var fowardForce = pitch * forwardSpeed / 10;      // fowards
            var sideWaysForce = strafe * sidewardSpeed / 10;  // sideways
                      
            roll = -(yaw + strafe);  //used for animations / mesh tilt: Not really accurate but this is an arcade type simulation - 

            ApplyMovingOrStoppingDragAndAngularDrag();                    
            ApplyMeshRotationsForRealism();

            rigidbody.AddRelativeTorque(0, yaw * rotationSpeed / 240, 0, ForceMode.VelocityChange);  
            
            rigidbody.AddRelativeForce(sideWaysForce, 0, fowardForce, ForceMode.VelocityChange);

            Debug.Log($"Yaw: {yaw}, Angular Velocity: {rigidbody.angularVelocity.y}");

            VerticalBoundaryForces();
        }





        /// <summary>
        /// This just speeds up the animation to counter the stroboscopic effect
        /// https://en.wikipedia.org/wiki/Wagon-wheel_effect
        /// </summary>
        private void CounterStroboscopicEffect()
        {
            if (yaw > 0)
            {
                this.GetComponentInChildren<Animation>()["Fly"].normalizedSpeed = 2f;
            }
            else
            {
                this.GetComponentInChildren<Animation>()["Fly"].normalizedSpeed = 1f;
            }
        }

        //Great forum post about rotated colliders and Unitys .bounds.Contains() and AABB limiations
        //Obtained from: https://forum.unity.com/threads/bounds-contains-is-not-working-at-all-as-expected.483628/
        bool ColliderContainsPoint(Transform ColliderTransform, Vector3 Point, bool Enabled)
        {
            Vector3 localPos = ColliderTransform.InverseTransformPoint(Point);
            if (Enabled && Mathf.Abs(localPos.x) < 0.5f && Mathf.Abs(localPos.y) < 0.5f && Mathf.Abs(localPos.z) < 0.5f)
                return true;
            else
                return false;
        }


        private void ApplyMeshRotationsForRealism()
        {
            rotationOfMesh = meshToTilt.rotation.eulerAngles;

            rotationOfMesh.x = ClampAngle(pitch * forwardSpeed, -_meshRotationAngle, _meshRotationAngle);
            rotationOfMesh.z = ClampAngle(roll * rotationSpeed  + strafe * sidewardSpeed, -_meshRotationAngle, _meshRotationAngle);

            meshToTilt.rotation = Quaternion.Euler(rotationOfMesh);
        }

        private void RestrictVerticalMovement()
        {
            if (rigidbody.position.y > maxHeight)
            {
                onCeiling = true;
                rigidbody.position = new Vector3(rigidbody.position.x, maxHeight, rigidbody.position.z);
            }
            else
            {
                onCeiling = false;
            }

            if (rigidbody.position.y <= minHeight)
            {
                onFloor = true;
                rigidbody.position = new Vector3(rigidbody.position.x, minHeight, rigidbody.position.z);
            }
            else
            {
                onFloor = false;
            }
        }

        /// <summary>
        /// Disable Drag if applying movement 
        /// </summary>
        private void ApplyMovingOrStoppingDragAndAngularDrag()
        {        
            if (yaw != 0)
            {
                rigidbody.angularDamping = 0;
            }
            else
            {
                rigidbody.angularDamping = stoppingAngularDrag;
            }

            //if (pitch != 0 || strafe != 0 || lift != 0)
            if (pitch != 0 || lift != 0)
            {
                rigidbody.linearDamping = 0;
            }
            else
            {
                rigidbody.linearDamping = stoppingDrag;
            }
        }

        private void VerticalBoundaryForces()
        {
            if (onCeiling || onFloor)
            {
                rigidbody.linearVelocity = new Vector3(rigidbody.linearVelocity.x, 0, rigidbody.linearVelocity.z);
            }
            else
            {
                rigidbody.AddForce(0, lift * verticalSpeed / 10, 0, ForceMode.VelocityChange);
            }

            if (onCeiling && lift < 0)
            {
                rigidbody.AddForce(0, lift, 0, ForceMode.Impulse);
            }
            if (onFloor && lift > 0)
            {
                rigidbody.AddForce(0, lift, 0, ForceMode.Impulse);
            }
        }

        //Angle Clamping is not handled with Unity's Mathf.Clamp so this solution was the best
        //Author: https://forum.unity.com/threads/limiting-rotation-with-mathf-clamp.171294/#post-2244265
        static float ClampAngle(float angle, float min, float max)
        {
            if (min < 0 && max > 0 && (angle > max || angle < min))
            {
                angle -= 360;
                if (angle > max || angle < min)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                    else return max;
                }
            }
            else if (min > 0 && (angle > max || angle < min))
            {
                angle += 360;
                if (angle > max || angle < min)
                {
                    if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                    else return max;
                }
            }

            if (angle < min) return min;
            else if (angle > max) return max;
            else return angle;
        }
    }
}