using UnityEngine;

namespace Scott.Barley.v2
{
    [CreateAssetMenu(menuName = "Upgrades/Upgrade Data Refill Ammo")]
    public class RefillAmmo_UpgradeDataSO : UpgradeActionSO
    {
        public override void fn_ApplyUpgrade(GameObject target)
        {
            Debug.Log("RefillAmmo_UpgradeDataSO fn_ApplyUpgrade Called");
            WeaponsUpgradeRelay_Singelton.Instance.fn_ReloadAll();
        }
    }
}