using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Scott.Barley.v2;
using Scott.Barley;
using UnityEngine.InputSystem;

namespace Scott.Barley.v2 {


    /// <summary>
    /// Description: Future Strike - Targeting_Manager: A script for managing which targets are within range & allowing the user to cycle though them, for Future Strike
    /// Author: Scott Barley
    /// Date: 30/05/2022
    /// Version: 3.01
    /// Requirements: 
    ///     Calls from 'Targetable' needs to be on the targeted object
    /// References: 
    ///     Ok'ish tutorial....Unity 3D - Targeting System Tutorial; https://www.youtube.com/watch?v=Ra4W0iYVA1g 
    /// </summary>


    public class TrgtMng_TargetManager : MonoBehaviour {

        [SerializeField] private PlayerInput _playerInput;


        // --- List of Event Triggering Collision Detectors ---
        public List<TrgtMng_TargetAcquisitionCollider> AllEnemyQueryListDetectors;          // List of TargetAcquisitionColliders the manager is checking; need to manually set where the detector is looking for the event call
        public List<TrgtMng_TargetAcquisitionCollider> FrontGunQueryListDetectors;

        // --- Target Lists for the Collider Sets ---    
        public List<GameObject> allEnemiesTarget_List;                                      // list of all targetable game objects, within all detector areas : For Missiles
        public List<GameObject> frontGun_EnemiesTarget_List;                                // list of all targetable game objects, within the detector area for the front gun 'Area B" : For Front Gun

        // --- Player Transform ---
        [SerializeField] Transform playerTransfrom;

        // --- Target Lock Vissable Indicators ---
        [SerializeField] GameObject frontgun_targetLockIndicator;
        private Transform frontgun_targetLockIndicator_Transfrom;
        [SerializeField] GameObject primary_targetLockIndicator;

        // --- Target(s) ---
        [SerializeField] Transform nonTargetLockTargetTransform;                            // Default gun targeting transform positions - These could be hard coded with Vec3's for more efficency, but for now/usablity will use transforms
        private GameObject target;                                                          // Player Controled Targeting System
        private Transform frontGun_TargetTransform;

        // -- Player Controled Targeting System Variables --
        private int target_listPostionID;  // track which list member is the current lockedOn targets
        private int target_GOID; // the game object ID of the target
        private bool isLockedOn; // track LockedOn state
        private bool targetLockisOverridden = true;
        




        private bool noTargetInvokeCalled; // used so the noTargetInvoke Is only called once till reset



        #region UI Output Events
        // Events - Posts Data to the Target Info UI 
        [SerializeField] public UnityEvent<int, int> UpdateTargetHealthInfo_CurrentMax_TargetInfoUI;
        [SerializeField] public UnityEvent NoTarget_TargetInfoUI;
        [SerializeField] public UnityEvent<string> UpdateTargetName_TargetInfoUI;
        #endregion



        // ---- Functions ----




        #region Setup ( Start, OnEnable, OnDisable ) : Listners & Basic Variables

        InputAction _autoLockInputAction;
        InputAction _previouseInputAction;
        InputAction _nextInputAction;


        private void Awake()
        {
            _autoLockInputAction = _playerInput.actions["AutoLock"];    // 'C'
            _previouseInputAction = _playerInput.actions["Previous"];   // '1'
            _nextInputAction = _playerInput.actions["Next"];            // '2'
        }


        private void Start()
        {
            frontgun_targetLockIndicator_Transfrom = frontgun_targetLockIndicator.transform;
            frontGun_TargetTransform = nonTargetLockTargetTransform;
            targetLockisOverridden = true;
        }

        private void OnEnable() {
           

            //Add targeted listens
            foreach (var currentEnemyQueryListDetector_all in AllEnemyQueryListDetectors) {
                currentEnemyQueryListDetector_all.OnListChange.AddListener(FetchListsFrom_ALL_TargetAcquisitionColliders);
            }

            foreach (var currentEnemyQueryListDetector_front in FrontGunQueryListDetectors)
            {
                currentEnemyQueryListDetector_front.OnListChange.AddListener(FetchListFrom_Front_TargetAcquisitionColliders);
            }


            isLockedOn = false;
            target_listPostionID = 0;
            primary_targetLockIndicator.SetActive(false);

            noTargetInvokeCalled = true;
            
        }

        private void OnDisable() {
            //Remove targeted listens
            foreach (var currentEnemyQueryListDetector in AllEnemyQueryListDetectors) {
                currentEnemyQueryListDetector.OnListChange.RemoveListener(FetchListsFrom_ALL_TargetAcquisitionColliders);
            }
            foreach (var currentEnemyQueryListDetector_front in FrontGunQueryListDetectors)
            {
                currentEnemyQueryListDetector_front.OnListChange.RemoveListener(FetchListFrom_Front_TargetAcquisitionColliders);
            }
        }
        #endregion


        public void Update() {

            //      ---- Key Inputs -----
            // Targeting Override
            /*
            if (Input.GetButtonDown("WepAutoLock"))
            {
                targetLockisOverridden =! targetLockisOverridden;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                TargetLock_CycleThroughTargets_Down();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                TargetLock_CycleThroughTargets_Up();
            }
            */


            if (_autoLockInputAction.triggered)
            {
                targetLockisOverridden = !targetLockisOverridden;
            }

            if (_autoLockInputAction.triggered)
            {
                TargetLock_CycleThroughTargets_Down();
            }
            if (_nextInputAction.triggered)
            {
                TargetLock_CycleThroughTargets_Up();
            }


            //      ---- Move Primary targetLockIndicator ----
            if (targetLockisOverridden)
            {
                primary_targetLockIndicator.SetActive(true);
                primary_targetLockIndicator.transform.position = nonTargetLockTargetTransform.position;
            }
            else
            {
                if (isLockedOn == true)
                {
                    primary_targetLockIndicator.SetActive(true);
                    if(target != null) primary_targetLockIndicator.transform.position = target.transform.position;
                }
            }

            //   ---- Move FrontGun targetLockIndicator ----
            if (targetLockisOverridden)
            {
                frontgun_targetLockIndicator_Transfrom.position = nonTargetLockTargetTransform.position;
            } 
            else
            {
                frontgun_targetLockIndicator_Transfrom.position = frontGun_TargetTransform.position;
            }
                
        }

        private void FixedUpdate()
        {
            if ((targetLockisOverridden == false) && (isLockedOn == false))
            {
                primary_targetLockIndicator.SetActive(false);
            }
        }


        private void TargetLock_CycleThroughTargets_Up() {

            if ((target_listPostionID + 1) <= (allEnemiesTarget_List.Count - 1)) {
                
                target_listPostionID++;
            }
            else {
                target_listPostionID = 0;
            }

            if(allEnemiesTarget_List.Count > 0)
            {
                target = allEnemiesTarget_List[target_listPostionID];
                target_GOID = target.GetInstanceID();
            }
            

        }

        private void TargetLock_CycleThroughTargets_Down() {
            if ((target_listPostionID - 1) >= 0) {
                target_listPostionID--;
            }
            else {
                target_listPostionID = (allEnemiesTarget_List.Count - 1);
            }
            if (allEnemiesTarget_List.Count > 0)
            {
                target = allEnemiesTarget_List[target_listPostionID];
                target_GOID = target.GetInstanceID();
            }
        }




        /// <summary>
        /// Function called by the 'OnListChange' Event 
        /// </summary>
        private void FetchListFrom_Front_TargetAcquisitionColliders() {
            frontGun_EnemiesTarget_List.Clear();
            foreach (var currentEnemyQueryListDetector in FrontGunQueryListDetectors)
            {
                frontGun_EnemiesTarget_List.AddRange(currentEnemyQueryListDetector.targetableEnemies);
            }
            // remove duplicates 
            frontGun_EnemiesTarget_List = frontGun_EnemiesTarget_List.Select(go => go).Distinct().ToList();

            // order by closets to player
            frontGun_EnemiesTarget_List = frontGun_EnemiesTarget_List.OrderBy(go => (go.transform.position - playerTransfrom.position).magnitude).ToList();


            // if the front gun collider list has a target inside, set the closest one to the player list position [0] to the target, else use the default targeting position
            if (frontGun_EnemiesTarget_List.Count > 0)
            {
                frontGun_TargetTransform = frontGun_EnemiesTarget_List[0].transform;
            }
            else
            {
                frontGun_TargetTransform = nonTargetLockTargetTransform;
            }

        }


        /// <summary>
        /// Function called by the 'OnListChange' Event 
        /// </summary>
        private void FetchListsFrom_ALL_TargetAcquisitionColliders() {

            allEnemiesTarget_List.Clear();
            foreach (var currentEnemyQueryListDetector in AllEnemyQueryListDetectors) {
                allEnemiesTarget_List.AddRange(currentEnemyQueryListDetector.targetableEnemies);
            }

            // remove duplicates 
            allEnemiesTarget_List = allEnemiesTarget_List.Select(go => go).Distinct().ToList();
            // order by InstanceID
            allEnemiesTarget_List = allEnemiesTarget_List.OrderByDescending(go => go.GetInstanceID()).ToList();

            if ((allEnemiesTarget_List.FirstOrDefault(go => go.GetInstanceID() == target_GOID) != null)){

                // target target_GOID was in list
                primary_targetLockIndicator.SetActive(true);
                isLockedOn = true;
            }
            else
            {
                // target target_GOID was NOT in the list, thus find a new target
                //Debug.Log("Could Not Find current target_GOID in list, will find new target");
                if (allEnemiesTarget_List.Count > 0)
                {
                    FindNewTarget();
                }
                else
                {
                    isLockedOn = false;
                }
            }

            if (allEnemiesTarget_List.Count <=0)
            {
                isLockedOn = false;
            }
      
            //Debug.Log(" Collider " + gameObject.name + " List Length: " + allEnemiesTarget_List.Count);
        }

        private void FindNewTarget()
        {
            primary_targetLockIndicator.SetActive(false);
            if (allEnemiesTarget_List.Count >= 1)
            {
                // set the first list item to the current target
                target_listPostionID = 0;
                target = allEnemiesTarget_List[target_listPostionID];
                target_GOID = target.GetInstanceID();

                isLockedOn = true;
            }
        }


    public Transform Get_TargetTransform() {
  
            // only return a target transform if the is a locked on target
            if (targetLockisOverridden) {
                return nonTargetLockTargetTransform;
            }
            else
            if ((target != null) && (isLockedOn == true)) {
                return target.transform;
            }
            else return null;
        }

        public Transform Get_FrontGunTargetTransform()
        {

            // only return a target transform if the is a locked on target
            if (targetLockisOverridden)
            {
                return nonTargetLockTargetTransform;
            }
            else
            if (frontgun_targetLockIndicator_Transfrom != null)
            {
                return frontgun_targetLockIndicator_Transfrom;
            }
            else return null;
        }





        #region UI Output Information

        
        private void TargetInfo_UI_Invoke_TargetHealth() {
            //Debug.Log("::  PostTargetInfoToUI_v1: Logic Check:: lockedOnTargetID, nearByTargets.Count :" + lockedOnTargetID + " , " + nearByTargets.Count); 

            // check if the lockedOnTarget has a ObjectHealth Component
            if (allEnemiesTarget_List[target_listPostionID].gameObject.GetComponent<ObjectHealth>()) {
                int currentHealth = allEnemiesTarget_List[target_listPostionID].gameObject.GetComponent<ObjectHealth>().Get_CurrentHealth();
                int maxHealth = allEnemiesTarget_List[target_listPostionID].gameObject.GetComponent<ObjectHealth>().Get_MaxHealth();
                UpdateTargetHealthInfo_CurrentMax_TargetInfoUI?.Invoke(currentHealth, maxHealth);
            }
        }
        private void TargetInfo_UI_Invoke_NoTarget() {
            NoTarget_TargetInfoUI?.Invoke();
        }
        

        #endregion



    }

}
