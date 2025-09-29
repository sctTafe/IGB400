using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {

    /// <summary>
    ///  NOT TESTED OR FINISHED YET!!!!
    ///  
    /// 
    /// To Add: 
    ///     Add a stage between 1 & 2, to change the 'rocketMaxAngularRotationSpeed' so the curving is slowering in that initial transition period 
    ///     Add functionality for turning Target Tracking On/Off
    ///     
    /// 
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile_VerticalLaunch : Projectile
    {
        [SerializeField] PooledParticalsActivation pooledParticalsActivation;
        [SerializeField] string CollisionParticalEffect_Tag = "PS_Explosion_A_V1";


        [SerializeField] float rocketMaxAngularRotationSpeed = 3f;
        [SerializeField] float rocketInitialLaunchSpeed = 5f;
        [SerializeField] float verticalAcceleration = 5f; //10m/s
        [SerializeField] float toTargetAcceleration = 10f; //10m/s
        [SerializeField] Vector3 launchAngleOffset = new Vector3(-90, 0, 0);
        [SerializeField] float stage1Time; // how long if flies just straight for

        [SerializeField] float debug_CurrentverticalAcceleration;



        private Transform targetTransform; // needs to be assigned when
        private float rocketMovmentSpeed;
        private Rigidbody rocketRigidbody;
        private Transform rocketTransform;
        private Quaternion rotationToTarget;
        private float launchStageTimer;
        private float launchStage2Timer;

        private float stage2time;
        private float currentMaxAngualrRotationSpeed;

        private bool isInState1;
        private bool isInState2;
        private bool isInState3;
        

        //debug
        float debugWaitTimer;
        float debugWait = 0.5f;

        // Event Calling for PS Effect
        //[SerializeField] public UnityEvent<string,Vector3, Quaternion> PooledParticalsActivation_DequeueParticalEffect_ByTag;
        //PS_Explosion_A_V1

        protected override void Awake()
        {
            base.Awake();

            pooledParticalsActivation = FindObjectOfType<PooledParticalsActivation>();
            rocketRigidbody = this.GetComponent<Rigidbody>();
            rocketTransform = this.transform;
            stage2time = stage1Time / 4;

            LaunchInitialSetUp();

            if (projectiles_Pool == null)
            {
                Debug.LogWarning("::Projectile::   did not find 'Projectiles_Pool.Instance' trying again in 'OnEnable'!");
                ConnectToProjectilesPool();
            }
        }

        private void Start()
        {
            LaunchInitialSetUp();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            LaunchInitialSetUp();
            if (!targetTransform)
            {
                Debug.LogWarning("::Projectile_VerticalLaunch:: Rocket Does Not Have Target!");
                return;
            }
            if (pooledParticalsActivation != null) 
            {
                pooledParticalsActivation = FindObjectOfType<PooledParticalsActivation>();
                Debug.LogWarning("::Projectile_VerticalLaunch:: Rocket pooledParticalsActivation Error!");
            }

        }

        private void LaunchInitialSetUp() {
            TimerForLaunchStages();
            rocketTransform.eulerAngles = launchAngleOffset;
            rocketMovmentSpeed = rocketInitialLaunchSpeed;
            rocketRigidbody.linearVelocity = Vector3.zero;
            rocketRigidbody.angularVelocity = Vector3.zero;
        }


        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            // launch Stage 1
            if (launchStageTimer >= Time.time)
            {
                if (!isInState1) State1();
                VerticalLaunch();
            }
            // launch Stage 2: Slow Rotation to Target Direction
            else if(launchStage2Timer >= Time.time)
            {
                if (!isInState2) State2();
                MoveToTarget();
            }
            // launch Stage 3
            else
            {
                if (!isInState3) State3();    
                MoveToTarget();
            }
        }


        protected override void AdditionalChildCodeToExecuteOnCollsion()
        {
            base.AdditionalChildCodeToExecuteOnCollsion();

            if (pooledParticalsActivation != null)
            {
                pooledParticalsActivation.DequeueParticalEffect_ByTag(CollisionParticalEffect_Tag, this.transform.position, transform.rotation);
            }
            
        }


        private void State1()  // Flying away from the Player
        {
            isInState1 = true;

        }

        private void State2()  // Turning to Face Target
        {
            isInState2 = true;
            currentMaxAngualrRotationSpeed = rocketMaxAngularRotationSpeed / 2;
        }

        private void State3() // Flying towards the Target
        {
            isInState3 = true;
            currentMaxAngualrRotationSpeed = rocketMaxAngularRotationSpeed;
        }



        private void VerticalLaunch()
        {
            //rotate projectile along the X by -90, so its pointing upward
            if (!rocketTransform) Debug.LogError("rocketTransform not present");
            rocketTransform.eulerAngles = launchAngleOffset;

            rocketMovmentSpeed += verticalAcceleration * Time.deltaTime;
            rocketRigidbody.linearVelocity = rocketTransform.forward * rocketMovmentSpeed;
        }

        private void MoveToTarget()
        {
            rocketMovmentSpeed += toTargetAcceleration * Time.deltaTime;
            rocketRigidbody.linearVelocity = rocketTransform.forward * rocketMovmentSpeed;
            rotationToTarget = Quaternion.LookRotation(targetTransform.position - rocketTransform.position); // LookRotation requred the object to be moving in the Foward for it to work correctly
            rocketRigidbody.MoveRotation(Quaternion.RotateTowards(rocketTransform.rotation, rotationToTarget, currentMaxAngualrRotationSpeed));
        }

        private void TimerForLaunchStages()
        {
            launchStageTimer = Time.time + stage1Time;
            launchStage2Timer = launchStageTimer + stage2time;
        }


        
        public void Set_TargetTransform(Transform targetTransformIn)
        {
            targetTransform = targetTransformIn;
        }


        private void DebugWait()
        {
            debugWaitTimer = Time.time + debugWait;
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
