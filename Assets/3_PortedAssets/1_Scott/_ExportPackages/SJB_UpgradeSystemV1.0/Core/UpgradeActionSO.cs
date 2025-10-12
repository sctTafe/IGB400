using UnityEngine;

public abstract class UpgradeActionSO : ScriptableObject, IUpgradeAction
{
    public string _upgradeName = "_name";
    public string _description = "_description";
    public Sprite _icon;

    public abstract void fn_ApplyUpgrade(GameObject target);
}