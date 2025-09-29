using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class Enemy_PlayerDetectionandTargetedShooting : MonoBehaviour
    {


        [SerializeField] string player_tag;
        [SerializeField] string weapon_tag;
        [SerializeField] float playerIsInRange_Distance = 20f;
        [SerializeField] bool requireLineOfSightToFire = false;
        [SerializeField] float fireRate_waitBetweenShots = 1f;
        [SerializeField] Transform point_ShootFromPoint_transform;

        private Transform transformOfPlayer;       
        private Projectiles_Pool projectiles_Pool;  // The Projectile Pool Instance
        private float timeToWaitTill;

        // Max Ammo?   // limit enemies to number of shots?
        // Events out for player warning

        private void Awake()
        {
            ConnectToProjectilesPool();  // calling a 'singleton' version of the class that is running  :::: check with Mark that is is correct & the correct way of doing it ::::
        }

        private void Start()
        {
            timeToWaitTill = 0;
        }


        // uses events, ask mark if this is cheaper or more expsnsive then using ??
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(player_tag))
            {
                transformOfPlayer = other.transform;
                if (Check_playerIsInRanger())
                {          
                    // if 'dosent require line of site', or 'line of sight not blocked by other object'
                    if ( !requireLineOfSightToFire || Check_PlayerIsInLineOfSight())
                    {
                        if (timeToWaitTill<= Time.time)
                        {
                            FireWeapon();
                            wait_betweenShot();
                        }
                    } 
                }
            }
        }

        // --- Private Functions ---
        #region Private Functions
        private bool Check_playerIsInRanger()
        {
            return (Vector3.Distance(transform.position, transformOfPlayer.position) < playerIsInRange_Distance) ? true : false;
        }

        private bool Check_PlayerIsInLineOfSight()
        {
            Vector3 direction = transformOfPlayer.position - point_ShootFromPoint_transform.position;
            float distance = direction.magnitude;
            Ray ray = new Ray(point_ShootFromPoint_transform.position, direction);
           
            Debug.DrawRay(ray.origin, ray.direction * distance);

            RaycastHit[] objectsAlongRay = Physics.RaycastAll(ray.origin, ray.direction, distance);

            
            if( objectsAlongRay.Length > 0)
            {
                // check if the ray is just picking up the player
                foreach (var item in objectsAlongRay)
                {
                    if (item.transform.CompareTag(player_tag) != true) return false;
                }

                return true;
            } else
            {
                return true;
            }

        }

        private void FireWeapon()            
        {
            if (projectiles_Pool == null) ConnectToProjectilesPool();
    
            GameObject projectileGO = projectiles_Pool.ProjectileDequeueFromPool(weapon_tag, point_ShootFromPoint_transform.position, Quaternion.identity);
            projectileGO.transform.position = point_ShootFromPoint_transform.position;
            projectileGO.transform.LookAt(transformOfPlayer);

            if (projectileGO.GetComponent<Projectile_VerticalLaunch>() != null)
            {
                Projectile_VerticalLaunch projectile = projectileGO.GetComponent<Projectile_VerticalLaunch>();
                projectile.Set_TargetTransform(transformOfPlayer);
                projectile.Set_TargetTrackingIsEnabled(true);
            }

            if (projectileGO.GetComponent<Projectile_WingLaunch>() != null)
            {
                Projectile_WingLaunch projectile = projectileGO.GetComponent<Projectile_WingLaunch>();
                projectile.Set_TargetTransform(transformOfPlayer);
                projectile.Set_TargetTrackingIsEnabled(true);
            }



        }

        private void ConnectToProjectilesPool()
        {
            if (Projectiles_Pool.Instance != null)
            {
                projectiles_Pool = Projectiles_Pool.Instance; //this is causing endless issues!!!!   - somthing to do with execution order???
            }
        }

        private void wait_betweenShot()
        {
            timeToWaitTill = Time.time + fireRate_waitBetweenShots;
        }

        #endregion
    }
}