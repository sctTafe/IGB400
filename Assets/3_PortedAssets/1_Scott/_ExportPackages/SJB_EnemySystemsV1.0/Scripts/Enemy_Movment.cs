using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class Enemy_Movment : MonoBehaviour
    {
        [Header("-- SetUp --")]
        [SerializeField] string player_tag;   
        //[SerializeField] float disengagementDistance_FromTarget;
        [SerializeField] bool requireLineOfSightToMoveToTarget = false;
        [SerializeField] Transform LOS_CheckPoinTransform;
        [SerializeField] Vector3 originPosition;
        [Header("SetUp - Functional Distances")]
        [SerializeField] float engagementDistance_ToTarget_yellow = 150f;
        [SerializeField] float disengagementDistance_FromOrigin_red = 200f;
        [Header("SetUp - Movement Values")]
        [SerializeField] float moveToTargetMaxSpeed;
        [SerializeField] float maxRotationSpeed;
        [SerializeField] float movementSlowingDistance_10pct;
        [SerializeField] float movementSlowingDistance_30pct;
        [Header("SetUp - Drunk Walk")]
        [SerializeField] float drunkWalkRadius_FromOrigin_green = 150f;
        [SerializeField] float drunkWalkRadius_timeBetweenNextPoint;
        [Header("Debug Values Out")]
        [SerializeField] bool debugON = false;
        public Vector3 current_MoveToPoint;
        public float current_distanceToPoint;
        public bool current_playerIsInRange;
        public bool current_enemyHasNotStrayedToFar;



        CharacterController enemyController;

        private Transform enemyTransform;
        private Transform targetTransform;
        private Vector3 lastSeenPosition;
        private Vector3 moveToPosition;


        bool playerIsInLineOfSight_bool;
        bool targetIsInEnguagmentRange_bool;
        bool enemyIsInEnguagmentRange_bool;
        bool enemyLostIntrestInTarget_bool;

        float wait_LOSLost_WaitTill;
        
        void Start()
        {
      
            enemyTransform = this.transform;
            originPosition = enemyTransform.position;
            enemyController = gameObject.AddComponent<CharacterController>();
        }




        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(player_tag))
            {
                targetTransform = other.transform;
                if (Check_playerIsInRanger())
                {
                    targetIsInEnguagmentRange_bool = true;

                    if (Check_EnemyIsLessThen_disengagementDistance_FromOrigin())
                    {
                        enemyIsInEnguagmentRange_bool = true;

                        // if 'dosent require line of site', or 'line of sight not blocked by other object'
                        if (requireLineOfSightToMoveToTarget == false || Check_PlayerIsInLineOfSight())
                        {
                            lastSeenPosition = targetTransform.position;
                            moveToPosition = lastSeenPosition;
                        }
                        else
                        {
                            // Lost Site of Target, but it is still in the area
                            Action_SetDrunkWalk_InCurrentArea();       
                        }
                    }
                    else
                    {
                        enemyIsInEnguagmentRange_bool = false;
                    }
                }
                else
                {
                    targetIsInEnguagmentRange_bool = false;
                }
            }
            else
            {
                enemyIsInEnguagmentRange_bool = false;
                targetIsInEnguagmentRange_bool = false;
            }
        }
        private void DebugValuesOut()
        {
            if (debugON)
            {
                current_playerIsInRange = targetIsInEnguagmentRange_bool;
                current_enemyHasNotStrayedToFar = enemyIsInEnguagmentRange_bool;
            }
        }


        private void FixedUpdate()
        {
            DebugValuesOut();

            if (targetIsInEnguagmentRange_bool == false || (targetIsInEnguagmentRange_bool == false && enemyIsInEnguagmentRange_bool == false))
            {
                if(wait_waitBetweenSettingNewDrunkWalkPoint_waitTill<= Time.time)
                {
                    Action_SetDrunkWalkTargetPoint();
                    wait_waitBetweenSettingNewDrunkWalkPoint();
                }
                
            }

            if (debugON) current_MoveToPoint = moveToPosition;
            Action_RotateToTarget();
            Action_MoveToTarget();

        }

        float wait_waitBetweenSettingNewDrunkWalkPoint_waitTill;
        private void wait_waitBetweenSettingNewDrunkWalkPoint()
        {
            wait_waitBetweenSettingNewDrunkWalkPoint_waitTill = Time.time + drunkWalkRadius_timeBetweenNextPoint;
        }



        private bool Check_playerIsInRanger()
        {
            return (Vector3.Distance(enemyTransform.position, targetTransform.position) < engagementDistance_ToTarget_yellow) ? true : false;
        }

        private bool Check_EnemyIsLessThen_disengagementDistance_FromOrigin()
        {
            return (Vector3.Distance(enemyTransform.position, originPosition) < disengagementDistance_FromOrigin_red) ? true : false;
        }

        private bool Check_PlayerIsInLineOfSight()
            {
                Vector3 direction = targetTransform.position - LOS_CheckPoinTransform.position;
                float distance = direction.magnitude;
                Ray ray = new Ray(LOS_CheckPoinTransform.position, direction);

                Debug.DrawRay(ray.origin, ray.direction * distance);

                RaycastHit[] objectsAlongRay = Physics.RaycastAll(ray.origin, ray.direction, distance);


                if (objectsAlongRay.Length > 0)
                {
                    // check if the ray is just picking up the player
                    foreach (var item in objectsAlongRay)
                    {
                        if (item.transform.CompareTag(player_tag) != true) return false;
                    }

                    return true;
                }
                else
                {
                    return true;
                }

            }
        
        private void Action_SetDrunkWalkTargetPoint()
        {  
            Vector3 drunkWalkPoint = originPosition + Random.insideUnitSphere * drunkWalkRadius_FromOrigin_green;
            drunkWalkPoint.y = enemyTransform.position.y;
            moveToPosition = drunkWalkPoint;
        }

        private void Action_SetDrunkWalk_InCurrentArea()
        {
            Vector3 drunkWalkPoint = enemyTransform.position + Random.insideUnitSphere * 5f;
            drunkWalkPoint.y = enemyTransform.position.y;
            moveToPosition = drunkWalkPoint;
        }

        private void Check_IsStuck()
        {

        }


        public void Action_RotateToTarget()
        {
            float rotationStep = maxRotationSpeed * Time.deltaTime;
            Quaternion rotationToTarget = Quaternion.LookRotation(moveToPosition - enemyTransform.position); // LookRotation requred the object to be moving in the Foward for it to work correctly
            enemyTransform.rotation = Quaternion.RotateTowards(enemyTransform.rotation, rotationToTarget, rotationStep);
        }


        private void Action_MoveToTarget()
        {
            float distance = Vector3.Distance(enemyTransform.position, moveToPosition);
            current_distanceToPoint = distance;

            if (distance <= movementSlowingDistance_30pct)
            {
                if (distance <= movementSlowingDistance_10pct)
                {
                    // Move at 30% Speed
                    enemyController.Move(enemyTransform.forward * Time.deltaTime * moveToTargetMaxSpeed * 0.1f);
                }
                else
                {
                    // Move at 10% Speed
                    enemyController.Move(enemyTransform.forward * Time.deltaTime * moveToTargetMaxSpeed * 0.3f);
                }
            }
            else
            {
                // Move At Full Speed
                enemyController.Move(enemyTransform.forward * Time.deltaTime * moveToTargetMaxSpeed);

            }
        }



        public void fnc_Set_OriginPosition(Vector3 position)
        {
            originPosition = position;
        }




        bool needToWait_LOSLost_bool;

        private void wait_LOS_Lost_3s()
        {
            wait_LOSLost_WaitTill = Time.time + 3f;
            needToWait_LOSLost_bool = true;
        }
        private void wait_LOS_Lost_3s_Anti()
        {
            if (wait_LOSLost_WaitTill <= Time.time)
            {
                needToWait_LOSLost_bool = false;
            }
        }



        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(originPosition, drunkWalkRadius_FromOrigin_green);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(originPosition, disengagementDistance_FromOrigin_red);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(originPosition, engagementDistance_ToTarget_yellow);
            
        }



    }
}