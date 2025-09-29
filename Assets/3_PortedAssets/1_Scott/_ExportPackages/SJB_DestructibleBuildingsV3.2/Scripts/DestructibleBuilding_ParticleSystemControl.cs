using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class DestructibleBuilding_ParticleSystemControl : MonoBehaviour
    {
        [SerializeField] bool debugON = false;
        [Header("Set Up")]
        private DestructibleBuilding_ObjectPooling destructibleBuilding_ObjectPooling;
        [SerializeField] string pooledPartical_Tag;
        [SerializeField] float particalEffect_LifeTime;
        private float disableParticalEffect_CountdownTime;




        private void Awake()
        {
            ConnectToObjectPoolInstance();

        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            if (destructibleBuilding_ObjectPooling != null) ConnectToObjectPoolInstance();
            disableParticalEffect_CountdownTime = Time.time + particalEffect_LifeTime;
        }


        private void OnDisable()
        {

        }


        private void FixedUpdate()
        {
            // if lifetime is up, disable the GameObject
            if (particalEffect_LifeTime > 0)
            {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOutOn");
                if (Time.time >= disableParticalEffect_CountdownTime)
                {
                    if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " RanOutOfTime & Returned to Pool");
                    fnc_RetrunToPool();
                }
            }
            else
            {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOut_OFF");
            }

        }

    private void ConnectToObjectPoolInstance()
        {
            if (DestructibleBuilding_ObjectPooling.Instance != null)
            {
                destructibleBuilding_ObjectPooling = DestructibleBuilding_ObjectPooling.Instance; //this is causing endless issues!!!!   - somthing to do with execution order???
            }
        }


        public void fnc_RetrunToPool()
        {
            if (destructibleBuilding_ObjectPooling == null) ConnectToObjectPoolInstance();

            if (destructibleBuilding_ObjectPooling != null)
            {
                if (debugON) { Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " Returned To Pool"); }
                destructibleBuilding_ObjectPooling.AddToPool(this.gameObject, pooledPartical_Tag);
                gameObject.SetActive(false);
            } else
            {
                Debug.LogError("::DestructibleBuilding_ParticleSystemControl:: Cannot Find its pool to return to!!!");
            }
           
        }

    }
}