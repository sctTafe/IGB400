using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{
    public class PooledParticalSettings : MonoBehaviour
    {
        [SerializeField] bool debugON = false;
        [SerializeField] string pooledPartical_Tag;
        [SerializeField] float particalEffect_LifeTime;


        protected Projectiles_Pool projectiles_Pool; // links to singleton instance
        private float disableParticalEffect_CountdownTime;






        protected virtual void Awake()
        {
            ConnectToProjectilesPool();
        }

        private void OnEnable()
        {
            
            if (projectiles_Pool == null)
            {
                Debug.LogWarning("::Projectile::   did not find 'Projectiles_Pool.Instance' trying again in 'OnEnable'!");
                ConnectToProjectilesPool();

                if (projectiles_Pool == null)
                {
                    return;
                }
            }
            disableParticalEffect_CountdownTime = Time.time + particalEffect_LifeTime;
        }

        private void OnDisable()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            // if lifetime is up, disable the GameObject
            if (particalEffect_LifeTime > 0)
            {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOutOn");
                if (Time.time >= disableParticalEffect_CountdownTime)
                {
                    if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " RanOutOfTime & Returned to Pool");
                    RetrunToPool();
                }
            }
            else
            {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOut_OFF");
            }

        }




        protected virtual void ConnectToProjectilesPool()
        {
            if (Projectiles_Pool.Instance != null)
            {
                projectiles_Pool = Projectiles_Pool.Instance; 
            }
        }

        private void RetrunToPool()
        {
            if (debugON) { Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " Returned To Pool"); }
            projectiles_Pool.AddToPool(this.gameObject, pooledPartical_Tag);
            gameObject.SetActive(false);
        }
    }
}