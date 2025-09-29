using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Description: Script needed for anything that needs to be targeted by the chopper .... might add life / health stats later
/// Author: Scott Barley
/// Requires: 'Shooting_TargetManager' to be in the game & only one of them - if i make this multiplayer this will cause errors.... i can fix this by linking it normally rather then searching for it.
/// 
/// Ref:
/// https://docs.unity3d.com/ScriptReference/Object.GetInstanceID.html - Unique Object ID
/// </summary>
public class Targetable : MonoBehaviour
{
    public bool alive;
    public int threatLevel;
    [SerializeField] bool isAddedToTargetManager;
    [SerializeField] int targetableInstanceID;
    //Shooting_TargetManager shooting_TargetManager;
    //Targetable_IDs targetable_IDs;

    // Start is called before the first frame update
    void Start()
    {
        isAddedToTargetManager = false;
        alive = true;
        targetableInstanceID = this.transform.gameObject.GetInstanceID();
        //shooting_TargetManager = Object.FindObjectOfType<Shooting_TargetManager>();

        /*
    targetable_IDs = Object.FindObjectOfType<Targetable_IDs>();
    targetableInstanceID = targetable_IDs.GetUniqueID();
    */
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddToTargetManager()
    {
        if (isAddedToTargetManager == false)
        {
           // Shooting_TargetManager.nearByTargets.Add(this);
            isAddedToTargetManager = true;
        }
    }

    public void RemoveFromTargetManager()
    {
        isAddedToTargetManager = false;
        //shooting_TargetManager.RemoveTargetFromList(targetableInstanceID);
        
    }

    public int Get_targetableInstanceID()
    {
        return targetableInstanceID;
    }


}
