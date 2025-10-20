using UnityEngine;

namespace Scott.Barley.v2
{
    [CreateAssetMenu(menuName = "Upgrades/Upgrade Data All Damage Up")]
    public class AllDmgUp_UpgradeDataSO : UpgradeActionSO
    {
        public override void fn_ApplyUpgrade(GameObject target)
        {
            Debug.Log("AllDmgUp_UpgradeDataSO fn_ApplyUpgrade Called");
            WeaponsUpgradeRelay_Singelton.Instance.fn_AllDmgUp(1.1f);
        }
    }
}