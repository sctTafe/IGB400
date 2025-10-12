using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Data Shoot Speed Up")]
public class ShootSpeedUp_UpgradeDataSO : UpgradeActionSO
{
    public override void fn_ApplyUpgrade(GameObject target)
    {
        WeaponsUpgradeRelay_Singelton.Instance.fn_AllShootSpeedUp(0.9f);
    }
}