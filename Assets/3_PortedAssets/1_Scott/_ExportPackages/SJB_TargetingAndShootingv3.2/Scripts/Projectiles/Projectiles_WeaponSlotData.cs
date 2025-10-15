using UnityEngine;

namespace Scott.Barley.v2 {
    public class Projectiles_WeaponSlotData : MonoBehaviour {


        public bool isAvalibleToPlayer;  // NOT USED YET:  add all weapons at the begining then use events to turn them on??? [Just an Idea]

        // if they are public they can just be changed without writing a function
        public string tag_ProjectilePoolRef;
        public bool hasTargetTracking; // for letting 'Projectile_Fire' know if to pass 'Target' Transform
        public int launcherType;   //which fire points / weapon launch points to use      
        public string weaponName;

        [Header("Ammo")]
        public int maxAmmo;
        public int remainingAmmo;

        [Header("Modifiable Value")]
        [SerializeField] int _baseDamageOnHit;
        [SerializeField] float _baseFireWeaponCooldown = 2f;


        public int CurrentDamageOnHit => _currentDamageOnHit;
        public float CooldownTimer_Acitve => _cooldownTimer_Active;

        //Internal
        float _currentfireCooldown;
        int _currentDamageOnHit;
        float _cooldownTimer_Active;
   


        //public GameObject projectile_Prefab;
        //public int intialProjectilePoolSize;


        [SerializeField] Projectiles_Fire projectiles_Fire;


        void Start()
        {
            _currentDamageOnHit = _baseDamageOnHit;
            _currentfireCooldown =  _baseFireWeaponCooldown;
        }



        public void fn_DmgUp(float pctChange)
        {
            _currentDamageOnHit = Mathf.RoundToInt(_currentDamageOnHit * pctChange);
        }

        public void fn_ShootSpeedUp(float pctChnage)
        {
            _currentfireCooldown *= pctChnage;
        }


        public void fn_SetFireCoolDown()
        {
            _cooldownTimer_Active = Time.time + _currentfireCooldown;
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