using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Scott.Barley.v2
{

    /// <summary>
    /// 
    /// 
    /// Need to:
    ///     write / update the logic for the mashine gun / laser
    ///     make sure the is a tracking mode  on/off both work
    ///     Increase projectile versitility: -
    ///         add a way to set damage amount on launch - will make the projectiles more versatile for using with enemies
    ///         add a way to set effect colours for projectiles
    ///         add a way for projectiles manuovrability to be set on launch
    /// 
    /// </summary>
    public class Projectiles_Fire : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;


        [SerializeField] bool DebugModeOn = false;
        [SerializeField] TrgtMng_TargetManager targetingManager;
        [SerializeField] GameObject PlayerGameObject;

        // Transforms of Initial Start positions of projectiles 'Launch Points'
        [SerializeField] Transform projectileLaunchTransform1_Front; // front cannon
        [SerializeField] Transform projectileLaunchTransform2_LeftWing; // rockets on wings
        [SerializeField] Transform projectileLaunchTransform3_RightWing; // rockets on wings
        [SerializeField] Transform projectileLaunchTransform4_Back; // vertical launcher


        // The Projectile Pool Instance
        private Projectiles_Pool projectiles_Pool;

        [Header("Weapons")]
        //Weapon Slot 1 variable 
        public Projectiles_WeaponSlotData weaponSlot1;
        public Projectiles_WeaponSlotData weaponSlot2;
        public Projectiles_WeaponSlotData weaponSlot3;
        public Projectiles_WeaponSlotData weaponSlot4;





        // On wings launch position flip flop bool: could use '%' but requires more code i think?
        private bool luancherType2LaunchPositionFLipFlopBool;

        //intial projectile movement velocity 
        private float initialForwardVelocity_LaunchPoints;
        private Rigidbody playerRigidbody_LaunchPointRigidBody;
        private Transform playerTransform;

        // old / test / obsolete variables - should remove for finished version
        [SerializeField] GameObject projectilePreFab;
        private float waitTillTime;
        [SerializeField] float shootCoolDownTime = 0.5f;
        private Vector3 shootFromPosition;
        [SerializeField] float projectileVelocity;
        int currentWeaponTypeID;

        [Header("JoyStickInputActions")]
        // Input State
        public bool _Up20_Action;
        public bool _Up80_Action;
        public bool _Down20_Action;
        public bool _Down80_Action;
        public bool _Left20_Action;
        public bool _Left80_Action;
        public bool _Right20_Action;
        public bool _Right80_Action;


        private void Awake()
        {
            projectiles_Pool = Projectiles_Pool.Instance;   // calling a 'singleton' version of the class that is running  :::: check with Mark that is is correct & the correct way of doing it ::::

        }
        private void Start()
        {
            playerRigidbody_LaunchPointRigidBody = PlayerGameObject.GetComponent<Rigidbody>();
            playerTransform = PlayerGameObject.transform;


        }
        private void OnEnable()
        {
            playerRigidbody_LaunchPointRigidBody = PlayerGameObject.GetComponent<Rigidbody>();
            playerTransform = PlayerGameObject.transform;
        }






        void Update()
        {
            Input_HandlePlayerInput();
            Input_HandleInputStates();
        }

        private void Input_HandlePlayerInput()
        {

            Vector2 input_Move = _playerInput.actions["Weapons"].ReadValue<Vector2>();
            float x = input_Move.x;
            float y = input_Move.y;

            _Up20_Action = _Up80_Action = false;
            _Down20_Action = _Down80_Action = false;
            _Left20_Action = _Left80_Action = false;
            _Right20_Action = _Right80_Action = false;

            // Vertical (Y)
            if (y > 0.2f && y < 0.8f)
                _Up20_Action = true;
            if (y >= 0.8f)
                _Up80_Action = true;

            if (y < -0.2f && y > -0.8f)
                _Down20_Action = true;
            if (y <= -0.8f)
                _Down80_Action = true;

            // Horizontal (X)
            if (x > 0.2f && x < 0.8f)
                _Right20_Action = true;
            if (x >= 0.8f)
                _Right80_Action = true;

            if (x < -0.2f && x > -0.8f)
                _Left20_Action = true;
            if (x <= -0.8f)
                _Left80_Action = true;

        }

        private void Input_HandleInputStates()
        {
            if (_Up20_Action)
                TryShootWeaponSlot(weaponSlot1, 1);
            if (_Up80_Action)
                TryShootWeaponSlot(weaponSlot1, 1);

            if (_Down20_Action)
                TryShootWeaponSlot(weaponSlot2, 2);
            if (_Down80_Action)
                TryShootWeaponSlot(weaponSlot2, 2);

            if (_Left20_Action)
                TryShootWeaponSlot(weaponSlot3, 3);
            if (_Left80_Action)
                TryShootWeaponSlot(weaponSlot3, 3);

            if (_Right20_Action)
                TryShootWeaponSlot(weaponSlot3, 3);
            if (_Right80_Action)
                TryShootWeaponSlot(weaponSlot3, 3);
        }


        private void FixedUpdate()
        {
            UpdateUI();
        }





        #region --- #Public Functions ---

        public void fn_Reload_WeponSlot1() => weaponSlot1.fnc_RefillAmmo();
        public void fn_Reload_WeponSlot2() => weaponSlot2.fnc_RefillAmmo();
        public void fn_Reload_WeponSlot3() => weaponSlot3.fnc_RefillAmmo();




        //public void fnc_AddWeaponToWeaponFireSlot(Projectiles_WeaponSlotData weaponType_Temp, int launcherType)
        //{
        //    if (launcherType == 1) // Guns & Lasers  (note: might want to split this later)
        //    {
        //        weaponSlot1.Add(weaponType_Temp);
        //    } else
        //    if (launcherType == 2) // Rocket
        //    {
        //        weaponSlot2.Add(weaponType_Temp);
        //    }
        //    else
        //    if (launcherType == 3) // Vertical Launch
        //    {
        //        weaponSlot2.Add(weaponType_Temp);
        //    } else
        //    {
        //        Debug.LogWarning("Projectiles_Fire: Warning! Tried to add a weapon with an incorrect launcherType, of: " + launcherType);
        //    }
        //}

        public void fn_ClearWeaponSlotsAll()
        {
            for (int i = 0; i < 4; i++) 
            {
                fn_ClearWeaponSlot(i+1);
            }

        }

        public void fn_ClearWeaponSlot(int index)
        {
            if(index == 1)
            {
                weaponSlot1 = null;
                return;
            }

            if (index == 2)
            {
                weaponSlot2 = null;
                return;
            }

            if (index == 3)
            {
                weaponSlot3 = null;
                return;
            }

            if (index == 4)
            {
                weaponSlot4 = null;
                return;
            }

            Debug.LogError($"No Weapon Slot by Index {index}");
        }



        #endregion

        #region --- #Private Functions ---

        private void UpdateUI()
        {
            if (weaponSlot1 != null)
            {
                UI_Weapons_Singleton.Instance.fn_WeaponSlot1(weaponSlot1.remainingAmmo);
            }

            if (weaponSlot2 != null)
            {
                UI_Weapons_Singleton.Instance.fn_WeaponSlot2(weaponSlot2.remainingAmmo);
            }

            if (weaponSlot3 != null)
            {
                UI_Weapons_Singleton.Instance.fn_WeaponSlot3(weaponSlot3.remainingAmmo);
            }
        }



        #region Cycle Through Weapons ws(1 & 2)

        //private void WeaponSlot1_CycleThroughAvalibleWeapons()
        //{
        //    if(weaponSlot1.Count > 0)
        //    {
        //        if ((ws1_CurrentID + 1) <= (weaponSlot1.Count - 1))
        //        {
        //            ws1_CurrentID++;
        //        }
        //        else
        //        {
        //            ws1_CurrentID = 0;
        //        }

        //        ws1_currentLancherType = weaponSlot1[ws1_CurrentID].launcherType;
        //        ws1_TargetTrackingOn = weaponSlot1[ws1_CurrentID].hasTargetTracking;

        //        Debug.Log("TEST_ weaponSlot1.count =" + weaponSlot1.Count);
        //        Debug.Log("weaponSlot1CurrentID: " + ws1_CurrentID + " tag: " + weaponSlot1[ws1_CurrentID].tag_ProjectilePoolRef);

        //        UpdateWeaponStatsUI_ws1CurrentAmmo?.Invoke(weaponSlot1[ws1_CurrentID].remainingAmmo);
        //        UpdateWeaponStatsUI_ws1TotalAmmo?.Invoke(weaponSlot1[ws1_CurrentID].maxAmmo);
        //        UpdateWeaponStatsUI_ws1Name?.Invoke(weaponSlot1[ws1_CurrentID].weaponName);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("::Projectiles_Fire:: WeaponSlot1_CycleThroughAvalibleWeapons; Nothing in Weapon Slot 1 to cycle through!");
        //    }
           

        //}

        //private void WeaponSlot2_CycleThroughAvalibleWeapons()
        //{
        //    if (weaponSlot2.Count > 0)
        //    {
        //        if ((ws2_CurrentID + 1) <= (weaponSlot2.Count - 1))
        //        {
        //            ws2_CurrentID++;
        //        }
        //        else
        //        {
        //            ws2_CurrentID = 0;
        //        }

        //        ws2_currentLancherType = weaponSlot2[ws2_CurrentID].launcherType;
        //        ws2_TargetTrackingOn = weaponSlot2[ws2_CurrentID].hasTargetTracking;

        //        Debug.Log("TEST_ weaponSlot2.count =" + weaponSlot2.Count);
        //        Debug.Log("weaponSlot2CurrentID: " + ws2_CurrentID + " tag: " + weaponSlot2[ws2_CurrentID].tag_ProjectilePoolRef);

        //        UpdateWeaponStatsUI_ws2CurrentAmmo?.Invoke(weaponSlot2[ws2_CurrentID].remainingAmmo);
        //        UpdateWeaponStatsUI_ws2TotalAmmo?.Invoke(weaponSlot2[ws2_CurrentID].maxAmmo);
        //        UpdateWeaponStatsUI_ws2Name?.Invoke(weaponSlot2[ws2_CurrentID].weaponName);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("::Projectiles_Fire:: WeaponSlot2_CycleThroughAvalibleWeapons; Nothing in Weapon Slot 2 to cycle through!");
        //    }
        //}

        #endregion

        #region Shoot Weapon ws(1 & 2)
        private void TryShootWeaponSlot(Projectiles_WeaponSlotData weaponSlotData, int UIIndex)
        {
            if (weaponSlotData == null)
                return;

            //If on cooldown ignore
            if (weaponSlotData._cooldownTimer > Time.time)
                return;
            

            var remainingAmmo = weaponSlotData.remainingAmmo;
            var projectilePoolRef = weaponSlotData.tag_ProjectilePoolRef;
            var launcherType = weaponSlotData.launcherType;
            var targetTracking = weaponSlotData.hasTargetTracking;


            if (remainingAmmo > 0)
            {
                if (launcherType != 1) 
                { 
                    if (NoTarget_ABORT_Check() == true) 
                        return; 
                }

                //---- IS SHOOTING ----
                Shoot_DequeueProjectileFromPoolByTagAndLauncherType_v2(weaponSlotData, projectilePoolRef, launcherType, targetTracking);
                //decrease the ammo
                weaponSlotData.fnc_DecressAmmoAmount(1);
                //trigger wait timer
                weaponSlotData.fn_SetFireCoolDown();
                //update UI
                UI_Weapons_Singleton.Instance.fn_WeaponSlotUpdate(UIIndex,remainingAmmo - 1);
            }                        
        }

        


        #endregion

        /// <summary>
        /// ABORT Firing if cannot receive data on what transformer should be fired at
        /// </summary>
        private bool NoTarget_ABORT_Check()
        {
            if (targetingManager.Get_TargetTransform() == null)
            {
                Debug.Log("Projectiles_Fire: Player Attempted to Fire when no target was allocated");
                return true;
            } else
            {
                return false;
            }
        }



        #region Shoot -> Dequeue Related Functions

        /// <summary>
        /// Dequeses the projectile from the 'Projectiles_Pool'
        /// </summary>
        /// <param name="currentWeaponTag"> This is used so the 'Projectiles_Pool' knows which object to retrive</param>
        /// <param name="launcherType"> This is used so the function knows which 'start' point; 'shootFromPosition' to use</param>
        private void Shoot_DequeueProjectileFromPoolByTagAndLauncherType_v2(Projectiles_WeaponSlotData wsd, string currentWeaponTag, int launcherType, bool targetTracking = false)
        {
            
            
            //// ABORT if cannot receive data on what transformer should be fired at
            //if (targetingManager.Get_TargetTransform() == null)
            //{
            //    Debug.Log("Projectiles_Fire: Player Attempted to Fire when no target was allocated");
            //    return;
            //}


            // --- Retrive Target Transform; used for 'shoot at' position ---
            Transform targetTransform;
            if (launcherType == 1) //If front gun '(launcherType == 1)', use the Front Gun Target Transfrom 
            {
                targetTransform = targetingManager.Get_FrontGunTargetTransform();
            } 
            else
            {
                targetTransform = targetingManager.Get_TargetTransform();
            }
            

            // get the firing point, based on the 'launcherType'; wing, back, front....etc.
            shootFromPosition = Shoot_Dequeue_LaunchPointBasedOnLauncherType(launcherType).position;     

            if (DebugModeOn) Debug.DrawLine(shootFromPosition, targetTransform.position, Color.blue, Mathf.Infinity);

            GameObject projectileGO = projectiles_Pool.ProjectileDequeueFromPool(currentWeaponTag, shootFromPosition, Quaternion.identity);
            //GameObject projectileGO = projectiles_Pool.ProjectileDequeueFromPool(currentWeaponTag, shootFromPosition, PlayerGameObject.transform.rotation);
            //projectileGO.transform.localRotation = Quaternion.identity;
            projectileGO.transform.position = shootFromPosition;
            projectileGO.transform.LookAt(targetTransform);

            Rigidbody rb = projectileGO.GetComponent<Rigidbody>();
            rb.linearVelocity = PlayerGameObject.transform.GetComponent<Rigidbody>().linearVelocity;    // this may be problematic???
            rb.angularVelocity = Vector3.zero;

            //Update the 'initialForwardVelocity_LaunchPoints'
            Shoot_Dequeue_LaunchPointVelocity();

            if (targetTracking)
            {
                if (DebugModeOn) Debug.Log("Projectile Target Tracking = " + targetTracking + ", Launch Type = " + launcherType);
                if (launcherType == 3) // Vertical Launcher
                {
                    var Projectile_VerticalLaunch = projectileGO.GetComponent<Projectile_VerticalLaunch>();
                    Projectile_VerticalLaunch.Set_TargetTransform(targetTransform);
                    Projectile_VerticalLaunch.Set_TargetTrackingIsEnabled(true);
                    Projectile_VerticalLaunch.Set_InitialMovementSpeed(initialForwardVelocity_LaunchPoints);
                    Projectile_VerticalLaunch.fn_SetDamageInflicted()
                } else
                if (launcherType == 2) // Horizontal Launcher
                {
                    projectileGO.GetComponent<Projectile_WingLaunch>().Set_TargetTransform(targetTransform);
                    projectileGO.GetComponent<Projectile_WingLaunch>().Set_TargetTrackingIsEnabled(true);
                    projectileGO.GetComponent<Projectile_WingLaunch>().Set_InitialMovementSpeed(initialForwardVelocity_LaunchPoints);
                }
                else
                if (launcherType == 1)
                {
                    // need to wite this still......
                    var Projectile_MashineGun = projectileGO.GetComponent<Projectile_MashineGun>();
                } else
                {
                    Debug.LogWarning("::Warning!:: Projectiles_Fire: Incorrect targetTracking setting of: " + targetTracking);
                }
                }
            else
            {
                Vector3 toTargetVector3 = (targetTransform.position - shootFromPosition).normalized;
                rb.AddRelativeForce(toTargetVector3 * projectileVelocity);
                if (DebugModeOn) Debug.DrawLine(projectileGO.transform.position, (toTargetVector3 * projectileVelocity), Color.red, Mathf.Infinity);
            }


            

        }
        private void Shoot_Dequeue_LaunchPointVelocity()
        {
            initialForwardVelocity_LaunchPoints = Vector3.Dot(playerRigidbody_LaunchPointRigidBody.linearVelocity, playerTransform.forward);
            //initialForwardVelocity_LaunchPoints = playerRigidbody_LaunchPointRigidBody.velocity.magnitude;
            if (DebugModeOn) Debug.Log("initialForwardVelocity_LaunchPoints = " + initialForwardVelocity_LaunchPoints);
        }


        private Transform Shoot_Dequeue_LaunchPointBasedOnLauncherType(int launcherType)
        {
            if (launcherType == 1) return projectileLaunchTransform1_Front;
            if (launcherType == 2) return Shoot_Dequeue_LaunchPoint_LauncherType2LaunchPositionFlipFLop();
            if (launcherType == 3) return projectileLaunchTransform4_Back;
            else
            {
                Debug.LogWarning("Warning! projectiles_Pool: Shoot_LaunchPointBasedOnLauncherType; Incorrect Launcher typo of: " + launcherType);
                return null;
            }
        }

        private Transform Shoot_Dequeue_LaunchPoint_LauncherType2LaunchPositionFlipFLop()
        {
            luancherType2LaunchPositionFLipFlopBool = !luancherType2LaunchPositionFLipFlopBool;
            if (luancherType2LaunchPositionFLipFlopBool)
            {
                return projectileLaunchTransform2_LeftWing;
            } else
            {
                return projectileLaunchTransform3_RightWing;
            }
        }

        #endregion

        #endregion
    }
}