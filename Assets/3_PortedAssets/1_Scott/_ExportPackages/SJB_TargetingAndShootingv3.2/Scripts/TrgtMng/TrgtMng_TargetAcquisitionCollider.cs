using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {

    /// <summary>
    /// Description: Scotts Future Strike Target Acquisition Collider
    /// Author: Scott Barley
    /// Requires: 'Targetable' scripts to be on anything that can actively targeted to be shot at
    /// </summary>
    

    //[RequireComponent(typeof(Rigidbody))]
    public class TrgtMng_TargetAcquisitionCollider : MonoBehaviour {

        public UnityEvent OnListChange;
        public List<GameObject> targetableEnemies = new List<GameObject>();


        private void OnTriggerEnter(Collider col) {
            if (col.gameObject.GetComponent<Targetable>()) {

                targetableEnemies.Add(col.gameObject);
                OnListChange?.Invoke();
                //ListUpdated_DebugOut();

            }
            
        }

        private void OnTriggerExit(Collider col) {
            if (col.gameObject.GetComponent<Targetable>()) {

                targetableEnemies.Remove(col.gameObject);
                OnListChange?.Invoke();
                //ListUpdated_DebugOut();

            }
        }

        // checks list on physics call 'fixed update' for anything thats been destoryed / is now 'null', if any list member is null, it called the list update event
        private void FixedUpdate() {
            foreach (var item in targetableEnemies) {
                if ((item == null) || (item.activeSelf == false)) {
                    targetableEnemies.Remove(item);
                    OnListChange?.Invoke();
                    //ListUpdated_DebugOut();
                }
            }

            
        }

        private void ListUpdated_DebugOut() {
            Debug.Log(" Collider " + gameObject.name + " List Length: " + targetableEnemies.Count);
        }

    }
}