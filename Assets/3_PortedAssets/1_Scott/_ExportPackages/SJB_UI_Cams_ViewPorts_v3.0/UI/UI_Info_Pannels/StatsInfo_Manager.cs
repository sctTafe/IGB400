using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class StatsInfo_Manager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI energy;
        [SerializeField] TextMeshProUGUI armour;
        [SerializeField] TextMeshProUGUI drones;

        public void fnc_Set_energy(int amount)
        {
            energy.text = amount.ToString();
            if (amount >= 30) energy.color = Color.black;
            if (amount <= 29) energy.color = Color.yellow;
            if (amount <= 10) energy.color = Color.red;
        }
        public void fnc_Set_armour(int amount)
        {
            armour.text = amount.ToString();
            if (amount >= 30) armour.color = Color.black;
            if (amount <= 29) armour.color = Color.yellow;
            if (amount <= 10) armour.color = Color.red;
        }
        public void fnc_Set_drones(int amount)
        {
            drones.text = amount.ToString();

            if (amount >= 3) drones.color = Color.black;
            if (amount <= 2) drones.color = Color.yellow;
            if (amount <= 1) drones.color = Color.red;
        }

    }
}