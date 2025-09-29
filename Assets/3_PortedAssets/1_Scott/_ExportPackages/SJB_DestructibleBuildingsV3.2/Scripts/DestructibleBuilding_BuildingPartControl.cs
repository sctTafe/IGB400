using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class DestructibleBuilding_BuildingPartControl : MonoBehaviour
    {
        [SerializeField] bool debugON = false;
        [Header("Set Up")]
        private DestructibleBuilding_ObjectPooling destructibleBuilding_ObjectPooling;
        [SerializeField] string pooledBuildingType_Tag;
        [SerializeField] float buildingPartsDequed_LifeTime;
        private float autoDisable_CountdownTime;

        private bool hasCapturedOriginPoints;
        public List<Vector3> childRelativeTransformsPositions;
        public List<Transform> childRelativeTransforms;

        private bool hasReturnedToPool; // Stops the Pool Spazzing out if the 'return to pool' function is repeatedly called

        private void Awake()
        {
            ConnectToObjectPoolInstance();
        }


        private void Start()
        {
            // on start log all the transform positions on the childen, to return them to their origonal points, after use
            hasCapturedOriginPoints = false;
            if (hasCapturedOriginPoints == false) CaptureChildRelativeTransformPositions();
        }

        private void OnEnable()
        {
            if (destructibleBuilding_ObjectPooling != null) ConnectToObjectPoolInstance();
            if (hasCapturedOriginPoints == false) CaptureChildRelativeTransformPositions();
            autoDisable_CountdownTime = Time.time + buildingPartsDequed_LifeTime;
            hasReturnedToPool = false;

        }


        private void OnDisable()
        {
            ReSetChildTransformPositions();
        }


        private void FixedUpdate()
        {
            // if lifetime is up, disable the GameObject
            if (buildingPartsDequed_LifeTime > 0)
            {
                if (debugON) Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " PojectileTimeOutOn");
                if (Time.time >= autoDisable_CountdownTime)
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

        private void CaptureChildRelativeTransformPositions()
        {
            childRelativeTransformsPositions.Clear();
            childRelativeTransforms.Clear();

            for (int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                // capture both, to ensure the matchup is correct when resetting
                childRelativeTransformsPositions.Add(transform.GetChild(i).transform.position - this.transform.position);
                childRelativeTransforms.Add(transform.GetChild(i));
            }
            hasCapturedOriginPoints = true;
        }

        private void ReSetChildTransformPositions()
        {
            for (int i = 0; i < childRelativeTransformsPositions.Count; i++)
            {
                childRelativeTransforms[i].position = this.transform.position + childRelativeTransformsPositions[i];
                childRelativeTransforms[i].rotation = Quaternion.Euler(Vector3.zero);
                if (debugON) Debug.Log(":: DestructibleBuilding_BuildingPartControl :: ReSetChildTransformPositions; Reset Part " + i);
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
                if (hasReturnedToPool == false)
                {
                    if (debugON) { Debug.Log("Projectile" + this.gameObject.GetInstanceID() + " Returned To Pool"); }
                    gameObject.SetActive(false);
                    destructibleBuilding_ObjectPooling.AddToPool(this.gameObject, pooledBuildingType_Tag);

                    hasReturnedToPool = true;
                }

            }
            else
            {
                Debug.LogError("::DestructibleBuilding_ParticleSystemControl:: Cannot Find its pool to return to!!!");
            }

        }
    }
}