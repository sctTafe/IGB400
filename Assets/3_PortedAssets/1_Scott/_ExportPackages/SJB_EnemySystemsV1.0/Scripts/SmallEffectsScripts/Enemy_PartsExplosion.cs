using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class Enemy_PartsExplosion : MonoBehaviour
    {
        [SerializeField] float explosiveForce = 1000f;
        [SerializeField] float upLiftForce = 5f;
        [SerializeField] float torqureForce = 0.1f;
        [SerializeField] float lifeTimeOfExploadedParts = 5f;
        [SerializeField] Transform explosionCenterPoint;
        [SerializeField] List<GameObject> partsToExplode;

        private bool explosionHasBeenTriggered;
        private void Start()
        {
            explosionHasBeenTriggered = false;
        }
        public void fnc_ExplodeTheGroupe()
        {
            if (explosionHasBeenTriggered == false)
            {
                AddRigidbodys();
                MakeAllTheListPartOrphans();
                ExplodeTheGroup();

                explosionHasBeenTriggered = true;
            }
        }

        private void AddRigidbodys()
        {
            foreach (GameObject item in partsToExplode)
            {
                item.AddComponent<Rigidbody>();
            }
        }

        private void MakeAllTheListPartOrphans()
        {
            foreach (GameObject item in partsToExplode)
            {
                item.transform.SetParent(null);
                Destroy(item, lifeTimeOfExploadedParts);
            }
        }

        private void ExplodeTheGroup()
        {
            Vector3 explosionPoint = explosionCenterPoint.position;
            foreach (GameObject go in partsToExplode)
            {
                Rigidbody rb = go.GetComponent<Rigidbody>();

                rb.AddExplosionForce(explosiveForce, explosionPoint, upLiftForce);

                rb.AddTorque((Vector3.one* torqureForce));
            }
        }

        //private void Update() {
        //    Test_ByKeyDown();
        //}

        //private void Test_ByKeyDown() {
        //    if (Input.GetKeyDown(KeyCode.Space)) {
        //        fnc_ExplodeTheGroupe();
        //    }

        //}
    }
}