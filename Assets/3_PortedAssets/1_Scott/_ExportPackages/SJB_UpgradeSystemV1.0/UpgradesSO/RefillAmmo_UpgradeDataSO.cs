using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Data Refill Ammo")]
public class RefillAmmo_UpgradeDataSO : UpgradeActionSO
{
    public override void fn_ApplyUpgrade(GameObject target)
    {
        WeaponsUpgradeRelay_Singelton.Instance.fn_ReloadAll();
    }
}
