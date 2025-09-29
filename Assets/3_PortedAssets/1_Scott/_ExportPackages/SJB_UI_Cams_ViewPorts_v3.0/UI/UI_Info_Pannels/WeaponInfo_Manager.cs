using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scott.Barley.v2;

namespace Scott.Barley.v2
{

    public class WeaponInfo_Manager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI ws1CurrentAmmo;
        [SerializeField] TextMeshProUGUI ws1MaxAmmo;
        [SerializeField] TextMeshProUGUI ws2CurrentAmmo;
        [SerializeField] TextMeshProUGUI ws2MaxAmmo;
        [SerializeField] TextMeshProUGUI ws1Name;
        [SerializeField] TextMeshProUGUI ws2Name;


        public void fnc_Set_ws1CurrentAmmo(int amount)
        {
            ws1CurrentAmmo.text = amount.ToString();
        }
        public void fnc_Set_ws1MaxAmmo(int amount)
        {
            ws1MaxAmmo.text = amount.ToString();
        }



        public void fnc_Set_ws2CurrentAmmo(int amount)
        {
            ws2CurrentAmmo.text = amount.ToString();
        }
        public void fnc_Set_ws2MaxAmmo(int amount)
        {
            ws2MaxAmmo.text = amount.ToString();
        }



        public void fnc_Set_ws1Name(string name)
        {
            ws1Name.text = name.ToString();
        }


        public void fnc_Set_ws2Name(string name)
        {
            ws2Name.text = name.ToString();
        }


    }
}
