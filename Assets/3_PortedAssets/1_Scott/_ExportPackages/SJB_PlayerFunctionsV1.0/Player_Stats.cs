using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{
    public class Player_Stats : MonoBehaviour
    {
        [SerializeField] float energyBurnRate_PerSecond;

        [SerializeField] int startingLives;

        [SerializeField] float startHealth;
        [SerializeField] float maxHealth;

        [SerializeField] float startEnergy;
        [SerializeField] float maxEnergy;

        private int currentLives;
        private float playerCurrentHealth;
        private float playerCurrentEnergy;

        public bool playerIsDead;

        //[SerializeField] UnityEvent<int> playerHealthChange_Event;
        //[SerializeField] UnityEvent<int> playerEnergyChange_Event;
        [SerializeField] UnityEvent player_IsDead;
        [SerializeField] UnityEvent player_HasNoLives;

        Weapons_AddAndClear_OnDeathAndTrigger weapons_AddAndClear_OnDeathAndTrigger;
        StatsInfo_Manager statsInfo_Manager;

        private float timeTowaitTill;
        private float timeTowaitTill_4Hz;

        private float energyBurnRate_4Hz;

        private int energy_ptc;
        private int armour_ptc;


        private void Start()
        {
            
            currentLives = startingLives;
            playerCurrentEnergy = startEnergy;
            playerCurrentHealth = startHealth;
            playerIsDead = false;
            energyBurnRate_4Hz = energyBurnRate_PerSecond / 4;
            ConnectTo__StatsInfoManager();
            ConnectTo__WeaponsAddAndClear_OnDeathAndTrigger();
            Update_UI_Lives();
        }

        private void FixedUpdate()
        {
            Flying_BurnEnergy();       
        }

        private void LateUpdate()
        {
            Update_UI_Output();
        }

        private void Flying_BurnEnergy()
        {
            if(timeTowaitTill_4Hz <= Time.time)
            {
                fnc_changeEnergy(energyBurnRate_4Hz);
                _Wait_4Hz();
            }
        }


        private void Update_UI_Output()
        {
            if (statsInfo_Manager == null) ConnectTo__StatsInfoManager();

            energy_ptc = Mathf.FloorToInt( (playerCurrentEnergy / maxEnergy)*100);
            armour_ptc = Mathf.FloorToInt( (playerCurrentHealth / maxHealth)*100);

            statsInfo_Manager?.fnc_Set_armour(armour_ptc);
            statsInfo_Manager?.fnc_Set_energy(energy_ptc);

        }

        private void Update_UI_Lives()
        {
            if (statsInfo_Manager == null) ConnectTo__StatsInfoManager();
            statsInfo_Manager?.fnc_Set_drones(currentLives);
        }



        private void Check_IfDead_IfPlayerHasRunOutOfHealth()
        {
            if(playerCurrentHealth <= 0)
            {
                if (timeTowaitTill <= Time.time)
                {
                    playerIsDead = true;
                    Action_IsDead_ResetValuesOnDeath();
                    _Wait_3Seconds();
                }
            }           
        }

        private void Check_IfDead_IfPlayerHasRunOutOfEnergy()
        {
            if (playerCurrentEnergy  <= 0)
            {
                if(timeTowaitTill <= Time.time)
                {
                    playerIsDead = true;
                    Action_IsDead_ResetValuesOnDeath();
                    _Wait_3Seconds();
                }

            }
        }

        public void fnc_changeHealth(float valueChange)
        {
            playerCurrentHealth += valueChange;
            if (playerCurrentHealth > maxHealth) playerCurrentHealth = maxHealth;
            Check_IfDead_IfPlayerHasRunOutOfHealth();
        }

        public void fnc_changeEnergy(float valueChange)
        {
            playerCurrentEnergy += valueChange;
            if (playerCurrentEnergy > maxEnergy) playerCurrentEnergy = maxEnergy;
            Check_IfDead_IfPlayerHasRunOutOfEnergy();
        }

        private void _Wait_3Seconds()
        {
            timeTowaitTill = Time.time + 3f;
        }
        private void _Wait_4Hz()
        {
            timeTowaitTill_4Hz = Time.time + 0.25f;
        }


        private void Action_IsDead_ResetValuesOnDeath()
        {
            player_IsDead?.Invoke();
            playerCurrentHealth = startHealth;
            playerCurrentEnergy = startEnergy;
            playerIsDead = false;
            currentLives--;
            Check_IfPlayerHasPositiveNumberOfLives();
            Update_UI_Lives();
            if (weapons_AddAndClear_OnDeathAndTrigger == null) ConnectTo__WeaponsAddAndClear_OnDeathAndTrigger();
            weapons_AddAndClear_OnDeathAndTrigger?.fnc_RemoveWeapons();
        }

        private void Check_IfPlayerHasPositiveNumberOfLives()
        {
            if(currentLives <= 0)
            {
                player_HasNoLives?.Invoke();
            }
        }


        private void ConnectTo__StatsInfoManager()
        {
            statsInfo_Manager = FindObjectOfType<StatsInfo_Manager>();

        }
        private void ConnectTo__WeaponsAddAndClear_OnDeathAndTrigger()
        {
            weapons_AddAndClear_OnDeathAndTrigger = FindObjectOfType<Weapons_AddAndClear_OnDeathAndTrigger>();
        }
    }
}


