using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class Weapons_AddAndClear_OnDeathAndTrigger : MonoBehaviour
    {

        [SerializeField] List<Projectiles_WeaponSlotData> WeaponList;
        [SerializeField] GameObject weaponsVisuals;


        private Projectiles_Fire projectiles_Fire;
        private bool weaponIsAdded;

        // Start is called before the first frame update
        void Start()
        {
            projectiles_Fire = FindObjectOfType<Projectiles_Fire>();
            weaponIsAdded = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void fnc_AddWeaponsToPlayer()
        {
            if (!weaponIsAdded)
            {
                foreach (Projectiles_WeaponSlotData item in WeaponList)
                {
                    item.fnc_AddWeaponTo_ProjectileFire();
                    item.fnc_RefillAmmo();
                }
                weaponIsAdded = true;
                weaponsVisuals.SetActive(true);
            }

        }

        public void fnc_RemoveWeapons()
        {
            projectiles_Fire.fn_ClearWeaponSlotsAll();
            weaponIsAdded = false;
            weaponsVisuals.SetActive(false);
        }





    }
}
