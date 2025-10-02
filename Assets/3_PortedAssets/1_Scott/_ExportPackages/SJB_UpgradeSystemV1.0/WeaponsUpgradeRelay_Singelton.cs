using Scott.Barley.v2;
using UnityEngine;

public class WeaponsUpgradeRelay_Singelton : Singleton<WeaponsUpgradeRelay_Singelton>
{
    [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_1;
    [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_2_AutoCannon;
    [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_3_MisVert;
    [SerializeField] Projectiles_WeaponSlotData _WeaponSlotData_4_MisWing;
}
