using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scott.Barley.v2;

namespace Scott.Barley.v2 {
    public class Projectiles_WeaponSlotData : MonoBehaviour {


        public bool isAvalibleToPlayer;  // NOT USED YET:  add all weapons at the begining then use events to turn them on??? [Just an Idea]

        // if they are public they can just be changed without writing a function
        public string tag_ProjectilePoolRef;
        public bool hasTargetTracking; // for letting 'Projectile_Fire' know if to pass 'Target' Transform
        public int maxAmmo;
        public int remainingAmmo;
        public int launcherType;   //which fire points / weapon launch points to use
        public float fireCooldown = 0.5f;
        public string weaponName;
        public float _cooldownTimer;


        //public GameObject projectile_Prefab;
        //public int intialProjectilePoolSize;


        [SerializeField] Projectiles_Fire projectiles_Fire;


        public void fn_SetFireCoolDown()
        {
            _cooldownTimer = Time.time + fireCooldown;
        }



        public void fnc_DecressAmmoAmount(int ammount) {
            if (ammount > 0) remainingAmmo -= ammount;
        }

        public void fnc_RefillAmmo() {
            remainingAmmo = maxAmmo;
        }

        public void fnc_ToggleAvalibility() {
            isAvalibleToPlayer = !isAvalibleToPlayer;
        }

        public string get_tagProjectilePoolRef() {
            return tag_ProjectilePoolRef;
        }

        public void fnc_AddWeaponTo_ProjectileFire()
        {
            Debug.LogError(" FUNCTION NOT IMPLMENTED");
            //projectiles_Fire.fnc_AddWeaponToWeaponFireSlot(this, launcherType);
        }


    }
}