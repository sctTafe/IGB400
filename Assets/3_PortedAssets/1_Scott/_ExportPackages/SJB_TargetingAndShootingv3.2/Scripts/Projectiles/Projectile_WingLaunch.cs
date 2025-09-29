using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{


    /// <summary>
    /// Author: Scott Barley
    /// </summary>

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile_WingLaunch : Projectile
    {

        [SerializeField] PooledParticalsActivation pooledParticalsActivation;
        [SerializeField] string CollisionParticalEffect_Tag = "PS_Explosion_A_V1";

        [SerializeField] float rocketMaxAngularRotationSpeed = 3f;
        [SerializeField] float rocketInitialLaunchSpeed = 3f;
        [SerializeField] float toTargetAcceleration = 3f; // 3m/s per s

        private Transform targetTransform; // needs to be assigned when
        private float rocketMovmentSpeed;
        private Rigidbody rocketRigidbody;
        private Transform rocketTransform;
        private Quaternion rotationToTarget;


        protected override void Awake()
        {
            base.Awake();

            pooledParticalsActivation = FindObjectOfType<PooledParticalsActivation>();
            rocketRigidbody = this.GetComponent<Rigidbody>();
            rocketTransform = this.transform; //cached variables for effiency  ::Ask Mark About This::
            LaunchInitialSetUp();
            if (projectiles_Pool == null)
            {
                Debug.LogWarning("::Projectile::   did not find 'Projectiles_Pool.Instance' trying again in 'OnEnable'!");
                ConnectToProjectilesPool();
            }

        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!targetTransform)
            {
                Debug.LogWarning("Rocket Does Not Have Target!");
                return;
            }
            if (pooledParticalsActivation != null)
            {
                pooledParticalsActivation = FindObjectOfType<PooledParticalsActivation>();
                Debug.LogWarning("::Projectile_VerticalLaunch:: Rocket pooledParticalsActivation Error!");
            }
            LaunchInitialSetUp();
        }


        private void LaunchInitialSetUp()
        {
            rocketMovmentSpeed = rocketInitialLaunchSpeed;
            rocketRigidbody.linearVelocity = Vector3.zero;
            rocketRigidbody.angularVelocity = Vector3.zero;
        }


        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (targetTrackingIsEnabled)
            {
                MoveToTarget();
            }
            else
            {
                rocketMovmentSpeed += toTargetAcceleration * Time.deltaTime;
            }

        }

        //called by parent class: 'projectile'
        protected override void AdditionalChildCodeToExecuteOnCollsion()
        {
            base.AdditionalChildCodeToExecuteOnCollsion();

            if (pooledParticalsActivation != null)
            {
                pooledParticalsActivation.DequeueParticalEffect_ByTag(CollisionParticalEffect_Tag, this.transform.position, transform.rotation);
            }

        }

        private void MoveToTarget()
        {
            rocketMovmentSpeed += toTargetAcceleration * Time.deltaTime;
            rocketRigidbody.linearVelocity = rocketTransform.forward * rocketMovmentSpeed;
            rotationToTarget = Quaternion.LookRotation(targetTransform.position - rocketTransform.position); // LookRotation requred the object to be moving in the Foward for it to work correctly
            rocketRigidbody.MoveRotation(Quaternion.RotateTowards(rocketTransform.rotation, rotationToTarget, rocketMaxAngularRotationSpeed));
        }

        public void Set_TargetTransform(Transform targetTransformIn)
        {
            targetTransform = targetTransformIn;
        }

        public override void Set_InitialMovementSpeed(float initalSpeed)
        {
            // sets projectiles movement speed to that of the launch objects + an inital 'rocketInitialLaunchSpeed', (only if player is moving forward)
            if (initalSpeed > 0)
            {
                rocketMovmentSpeed = initalSpeed + rocketInitialLaunchSpeed;
            }
            base.Set_InitialMovementSpeed(initalSpeed);
        }

    }



}