using UnityEngine;

namespace Scott.Barley.v2
{
    [CreateAssetMenu(menuName = "Upgrades/Upgrade Data Shoot Speed Up")]
    public class ShootSpeedUp_UpgradeDataSO : UpgradeActionSO
    {
        public override void fn_ApplyUpgrade(GameObject target)
        {
            Debug.Log("ShootSpeedUp_UpgradeDataSO fn_ApplyUpgrade Called");
            WeaponsUpgradeRelay_Singelton.Instance.fn_AllShootSpeedUp(0.9f);
        }
    }
}