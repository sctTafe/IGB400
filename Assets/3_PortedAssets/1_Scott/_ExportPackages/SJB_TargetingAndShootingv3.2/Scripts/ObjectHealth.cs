using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{
    /// <summary>
    /// Heath Script to go on anything that will take damage
    /// Added a rigid body for the sake of Unity collision physics
    /// </summary>

    //[RequireComponent(typeof(Rigidbody))]
    public class ObjectHealth : MonoBehaviour
    {
        [SerializeField] bool DebugON = false;
        [SerializeField] bool isOnPlayer = false;
        [SerializeField] bool missionTargetMode;
        [SerializeField] bool buildingMode;

        [SerializeField] int maxHealth;
        [SerializeField] int currentHealth; // only SerializeField for debug
        [SerializeField] float state2_lightDmg_triggerPercentage;
        [SerializeField] float state3_MedDmg_triggerPercentage;
        [SerializeField] float state4_HvyDmg_triggerPercentage;

        //private MissionList missionList;

        private float _wait_timeToWaitTill_ToTakeDamageAgain;
        private float remaingPersentageHealth;
        private int currentStateID;

        // building Destruction
        private DestructibleBuildingControler cached_DestructibleBuildingControler;

        // Events
        [SerializeField] public UnityEvent LowDamageTrigger;
        [SerializeField] public UnityEvent MedDamageTrigger;
        [SerializeField] public UnityEvent HighDamageTrigger;
        [SerializeField] public UnityEvent DeathTrigger;

        [Header("Score")]
        [SerializeField] private int _OnDeathScoreValue; 

        // Is On Player
        Player_Stats player_Stats;
        private Score_TriggerRelay _ScoreTrigger;

        private void Start()
        {
            // missionList = GameObject.Find("ScriptHolder_Manager").GetComponent<MissionList>();
            cach_DestructibleBuildingControler();
            _ScoreTrigger = GetComponent<Score_TriggerRelay>();
        }
     

        private void OnEnable()
        {
            if (isOnPlayer)
            {
                maxHealth = 999999;
                ConnectTo__Player_Stats();

            }
            currentHealth = maxHealth;
            remaingPersentageHealth = 100f;
            CheckAndUpdateBuildingState();
        }
        
        public int Get_CurrentHealth()
        {
            return currentHealth;
        }
        public int Get_MaxHealth()
        {
            return maxHealth;
        }


        public void fnc_RemoveHealth(int damage)
        {
            

            if (damage > 0)
            {
                if (Time.time >= _wait_timeToWaitTill_ToTakeDamageAgain) {
                    currentHealth -= damage;

                    if (isOnPlayer)
                    {
                        float damage_f = -damage * 1f;
                        if (player_Stats == null) ConnectTo__Player_Stats();
                        player_Stats?.fnc_changeHealth(damage_f);
                        currentHealth = 999999;
                    }

                    _wait_IFrames();
                }
                
            }

            // only bother if the target has any health left
            if (currentHealth > 0)
            {
                if (DebugON) Debug.Log("::ObjectHealth:: fnc_RemoveHealth; " + damage + " damage done!; Remaining Health = " + currentHealth +"; its currently at: " + remaingPersentageHealth + " % health!");
                // calculate new health percentage remaining
                remaingPersentageHealth = ( (currentHealth*1f) / (maxHealth*1f)) * 100;
                CheckAndUpdateBuildingState();

            } else {
                if (DebugON) Debug.Log("::ObjectHealth:: fnc_RemoveHealth; " + damage + " damage done!; Target is Dead: with health = " + currentHealth);
                remaingPersentageHealth = -1;
                HealthState4_Destroyed();
            }
        }

        // Put State Related Coding in these functions 
        #region States - State Update Coding: 

        // Change Mesh, Turn On/Off Particle Effects, Change Textures, ect. 
        private void HealthState1_NoDamage()
        {
            if(DebugON) Debug.Log("Little to No Damage Taken By: " + transform.gameObject.name + "; its currently at: " + remaingPersentageHealth + "% health!");
            
        }
        private void HealthState2_LightDamage()
        {
            if (DebugON) Debug.Log("Light Damage Taken By: " + transform.gameObject.name + "; its currently at: " + remaingPersentageHealth + "% health!");
            LowDamageTrigger?.Invoke();
            IF_BuildingCollapseScriptAttatched_SetUp_DamageParticleEffectsPoints();
        }
        private void HealthState3_MedDamage()
        {
            if (DebugON) Debug.Log("Med Damage Taken By: " + transform.gameObject.name + "; its currently at: " + remaingPersentageHealth + "% health!");
            MedDamageTrigger?.Invoke();
            IF_BuildingCollapseScriptAttatched_TriggerDamageEffect();
        }
        private void HealthState3_HeavyDamage()
        {
            if (DebugON) Debug.Log("Heavy Damage Taken By: " + transform.gameObject.name + "; its currently at: " + remaingPersentageHealth + "% health!");
            HighDamageTrigger?.Invoke();
        }
        private void HealthState4_Destroyed()
        {
            if (DebugON) Debug.Log("Extream Damage Taken By: Target IS DEAD!!!!: " + transform.gameObject.name + "; its currently at: " + remaingPersentageHealth + "% health!");
            DeathTrigger?.Invoke();

            if (_ScoreTrigger != null)
                _ScoreTrigger.fn_AddScore(_OnDeathScoreValue);



            if (missionTargetMode) {
                Invoke_targetKill_MissionUI();
            }
            
            if (buildingMode == false) {
                Destroy(this.gameObject);
            } else
            {
                IF_BuildingCollapseScriptAttatched_TriggerCollapse();
            }

        }

        #endregion


        #region Private Functions

        private void ValidateCurrentState() {

        }

        private void Invoke_targetKill_MissionUI()
        {
           // missionList.fnc_JetPackEnemyUpdate(1);
        }


        private void CheckAndUpdateBuildingState()
        {
            // State 1 Check - No Damage
            if (remaingPersentageHealth > state2_lightDmg_triggerPercentage)
            {
                if (currentStateID != 1)
                {
                    HealthState1_NoDamage();
                }

            }
            else
            // State 2 Check  - Light Damage
            if ((remaingPersentageHealth < state2_lightDmg_triggerPercentage) && (remaingPersentageHealth > state3_MedDmg_triggerPercentage))
            {
                if (currentStateID != 2)
                {
                    HealthState2_LightDamage();
                }
            }
            else
            // State 3 Check - Med Damage
            if ((remaingPersentageHealth < state3_MedDmg_triggerPercentage) && (remaingPersentageHealth > state4_HvyDmg_triggerPercentage))
            {
                if (currentStateID != 3)
                {
                    HealthState3_MedDamage();
                }
            }
            else
            // State 4 Check - Heavy Damage
            if ((remaingPersentageHealth < state4_HvyDmg_triggerPercentage) && (remaingPersentageHealth > 0))
            {
                if (currentStateID != 4)
                {
                    HealthState3_HeavyDamage();
                }
            }
            else if (remaingPersentageHealth <= 0)
            // State 5 - Object Destoryed
            {
                
                    HealthState4_Destroyed();
                
            }
            else {
                Debug.LogWarning("### ObjectHealth: Bad Call in CheckAndUpdateBuildingState! ###");
            }
        }


        void _wait_IFrames() {
            _wait_timeToWaitTill_ToTakeDamageAgain = Time.time + 0.2f;
        }


        private void ConnectTo__Player_Stats()
        {
            player_Stats = FindObjectOfType<Player_Stats>();
        }


        #endregion


        #region Building Destruction Functions
        // Trigger Building Collapse Script Functions

        private void cach_DestructibleBuildingControler()
        {
            if (this.transform.gameObject.GetComponent<DestructibleBuildingControler>() != null)
            {
                cached_DestructibleBuildingControler = this.transform.gameObject.GetComponent<DestructibleBuildingControler>();
            }
        }
        private void IF_BuildingCollapseScriptAttatched_SetUp_DamageParticleEffectsPoints()
        {
            if (cached_DestructibleBuildingControler != null)
            {
                cached_DestructibleBuildingControler.fnc_SetUp_DamageParticleEffectsPoints();
                Debug.Log("ObjectHealth: Object Had CollapsingBuilding attatched; fnc_SetUp_DamageParticleEffectsPoints called.");
            }
        }
        private void IF_BuildingCollapseScriptAttatched_TriggerDamageEffect() {
            if (cached_DestructibleBuildingControler != null) {
                cached_DestructibleBuildingControler.fnc_triggerDamageEffect();
                Debug.Log("ObjectHealth: Object Had CollapsingBuilding attatched; fnc_triggerDamageEffect called.");
            }
        }
        private void IF_BuildingCollapseScriptAttatched_TriggerCollapse() {
            if (cached_DestructibleBuildingControler != null) {
                cached_DestructibleBuildingControler.fnc_triggerBuildingCollapse();
                Debug.Log("ObjectHealth: Object Had CollapsingBuilding attatched; fnc_triggerBuildingCollapse called.");
            }
        }

        #endregion

    }




}