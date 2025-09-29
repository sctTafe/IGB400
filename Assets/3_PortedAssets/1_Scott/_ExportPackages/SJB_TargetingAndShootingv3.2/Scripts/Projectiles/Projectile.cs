using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {
    /// <summary>
    /// 
    /// Requirments: NEEDS TO LInk WITH 'Projectile_Pool' & 'Projectile_Fire'
    /// 
    /// Goes on Projectile Prefab, for use when activating
    /// </summary>
    
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        private bool debugON = false;
        
        [SerializeField] string projectPoolTag;    // NEEDED FOR LINKING WITH 'Projectile_Pool' & 'Projectile_Fire'
        [SerializeField] float projectileLifetime;
        [SerializeField] int projectileDamageInflicted;

        // protected Varliable
        protected float initialMovementSpeed;
        protected bool targetTrackingIsEnabled;

        // private variables
        private float disableProjectileCountdown;
        protected Projectiles_Pool projectiles_Pool; // links to singleton instance
        private bool missleHasHitSomthing;



        protected virtual void AdditionalChildCodeToExecuteOnCollsion()
        {

        }

        /// <summary>
        ///  CodeToExecuteOnCollsion
        /// </summary>
        /// <param name="col"></param>
        protected virtual void CodeToExecuteOnCollsion(Collision col)
        {
            Debug.Log("::Projectile:: Projectile Hit = " + col.gameObject.name);
            if(col.gameObject.GetComponent<ObjectHealth>())
            {

                if (missleHasHitSomthing == false)
                {
                    missleHasHitSomthing = true;
                    col.gameObject.GetComponent<ObjectHealth>().fnc_RemoveHealth(projectileDamageInflicted);
                    //Debug.Log("Projectile Hit Somthing: WIth 'ObjectHealth', " + projectileDamageInflicted + " health has been removed!");
                }
                
            }
        }

        protected virtual void Awake()
        {
            ConnectToProjectilesPool();
        }

        protected virtual void ConnectToProjectilesPool()
        {
            if (Projectiles_Pool.Instance != null)
            {
                projectiles_Pool = Projectiles_Pool.Instance; //this is causing endless issues!!!!   - somthing to do with execution order???
            }
        }


        protected virtual void OnEnable()
        {
            if (projectiles_Pool == null) {
                Debug.LogWarning("::Projectile::   did not find 'Projectiles_Pool.Instance' trying again in 'OnEnable'!");
                ConnectToProjectilesPool();
            }

            // starts timered till return to pool, onEnable
            initialMovementSpeed = 0;
            disableProjectileCountdown = Time.time + projectileLifetime;
            missleHasHitSomthing = false;
        }
        protected virtual void OnDisable()
        {
            missleHasHitSomthing = false;
            initialMovementSpeed = 0;
        }



        protected virtual void FixedUpdate()
        {
            // if lifetime is up, disable the GameObject
            if (projectileLifetime > 0)
            {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOutOn");
                if (Time.time >= disableProjectileCountdown) {
                    if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " RanOutOfTime & Returned to Pool");
                    RetrunToPool();
                }
            } else {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOut_OFF");
            }
                   
        }

        void OnCollisionEnter(Collision collision)
        {
            if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " Hit");
            AdditionalChildCodeToExecuteOnCollsion();
            CodeToExecuteOnCollsion(collision);

            // Returns to pool on any collision: Run any interaction code prior to this.
            RetrunToPool();
        }

        private void RetrunToPool()
        {
            if (debugON) { Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " Returned To Pool"); }
            projectiles_Pool.AddToPool(this.gameObject, projectPoolTag);
            gameObject.SetActive(false);
        }

        public virtual void Set_InitialMovementSpeed(float initalSpeed)
        {
            initialMovementSpeed = initalSpeed;
        }
        public void Set_TargetTrackingIsEnabled(bool isEnabled)
        {
            targetTrackingIsEnabled = isEnabled;
        }
    }



}