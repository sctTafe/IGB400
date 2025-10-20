using System.Collections.Generic;
using UnityEngine;

namespace Scott.Barley.v2
{
    public class WeaponsUpgradeRelay_Singelton : Singleton<WeaponsUpgradeRelay_Singelton>
    {
        [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_1;
        [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_2_AutoCannon;
        [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_3_MisVert;
        [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_4_MisWing;

        List<Projectiles_WeaponSlotData> _weaponSlotsList = new();

        private void Awake()
        {
            _weaponSlotsList.Clear();

            if (_WeaponSlotData_1)
                _weaponSlotsList.Add(_WeaponSlotData_1);
            if (_WeaponSlotData_2_AutoCannon)
                _weaponSlotsList.Add(_WeaponSlotData_2_AutoCannon);
            if (_WeaponSlotData_3_MisVert)
                _weaponSlotsList.Add(_WeaponSlotData_3_MisVert);
            if (_WeaponSlotData_4_MisWing)
                _weaponSlotsList.Add(_WeaponSlotData_4_MisWing);


            //Reload(_WeaponSlotData_1);
            //Reload(_WeaponSlotData_2_AutoCannon);
            //Reload(_WeaponSlotData_3_MisVert);
            //Reload(_WeaponSlotData_4_MisWing);
        }

        //  Apply to all weapons types
        public void fn_ReloadAll()
        {
            foreach (var ws in _weaponSlotsList)
            {
                Reload(ws);
            }
        }

        public void fn_AllDmgUp(float pctChange)
        {
            foreach (var ws in _weaponSlotsList)
            {
                DmgUp(ws);
            }
        }

        public void fn_AllShootSpeedUp(float pctChange)
        {
            foreach (var ws in _weaponSlotsList)
            {
                ShootSpeedUp(ws);
            }
        }



        void Reload(Projectiles_WeaponSlotData wsd)
        {
            if (wsd == null)
                return;

            wsd.fnc_RefillAmmo();
        }

        void DmgUp(Projectiles_WeaponSlotData wsd, float pctChange = 1.10f)
        {
            if (wsd == null)
                return;

            wsd.fn_DmgUp(pctChange);
        }

        void ShootSpeedUp(Projectiles_WeaponSlotData wsd, float pctChange = 0.9f)
        {
            if (wsd == null)
                return;

            wsd.fn_ShootSpeedUp(pctChange);
        }
    }
}