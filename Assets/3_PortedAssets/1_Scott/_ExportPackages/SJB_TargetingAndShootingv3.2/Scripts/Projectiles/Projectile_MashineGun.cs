using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile_MashineGun : Projectile {


        [SerializeField] float baseProjectileSpeed = 100;

        private float bulletMovmentSpeed = 0;
        private Rigidbody bulletRigidbody;
        private Transform bulletTransform;

        protected override void Awake() {
            base.Awake();
            bulletRigidbody = this.GetComponent<Rigidbody>();
            bulletTransform = this.transform;
            if (projectiles_Pool == null)
            {
                Debug.LogWarning("::Projectile::   did not find 'Projectiles_Pool.Instance' trying again in 'OnEnable'!");
                ConnectToProjectilesPool();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (projectiles_Pool == null)
            {
                Debug.LogWarning("::Projectile::   did not find 'Projectiles_Pool.Instance' trying again in 'OnEnable'!");
                ConnectToProjectilesPool();
            }

            if (bulletRigidbody != null)
            {
                bulletRigidbody.linearVelocity = Vector3.zero;
                bulletRigidbody.angularVelocity = Vector3.zero;
                //Debug.Log("Projectile_MashineGun: bulletRigidbody zero'ed");
            }
            else
            {
                Debug.LogWarning("Projectile_MashineGun: No Rigid Body");
            }
            bulletMovmentSpeed = baseProjectileSpeed;

        }

        protected override void FixedUpdate() {
            base.FixedUpdate();
            
            bulletRigidbody.linearVelocity = bulletTransform.forward * bulletMovmentSpeed;
        }

        
        //public override void Set_InitialMovementSpeed(float initalSpeed) {
        //    // sets projectiles movement speed to that of the launch objects + an inital 'rocketInitialLaunchSpeed', (only if player is moving forward)
        //    bulletMovmentSpeed += initalSpeed;
        //}

    }
}