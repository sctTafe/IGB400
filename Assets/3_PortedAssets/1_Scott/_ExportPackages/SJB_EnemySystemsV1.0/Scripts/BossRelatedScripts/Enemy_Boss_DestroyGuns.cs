using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {
    public class Enemy_Boss_DestroyGuns : MonoBehaviour {

        public void fnc_DestroyTheGameobjectThisIsAttachedTo() {

            Destroy(this.transform.gameObject);

        }


    }
}