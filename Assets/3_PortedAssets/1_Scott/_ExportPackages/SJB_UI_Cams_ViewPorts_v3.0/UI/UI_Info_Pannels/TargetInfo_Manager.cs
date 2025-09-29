using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scott.Barley;

namespace Scott.Barley
{

    public class TargetInfo_Manager : MonoBehaviour
    {
        [SerializeField] GameObject TargetInfo_GameObject;
        [SerializeField] GameObject HealthText_GameObject;

        [SerializeField] Slider healthBarLtoRslider;
        [SerializeField] Slider healthBarRtoLslider;
        
        [SerializeField] TextMeshProUGUI healthBarcurrentHealth;
        [SerializeField] TextMeshProUGUI healthBarmaxHealth;

        private int percentageRemaining_thousandths;

        public void Set_HealthBar(int currentHealth, int maxHealth)
        {
            if(TargetInfo_GameObject.activeSelf == false) TargetInfo_GameObject.SetActive(true);

            percentageRemaining_thousandths = Mathf.FloorToInt(((currentHealth * 1f) / (maxHealth * 1f)) * 1000);
            healthBarLtoRslider.value = percentageRemaining_thousandths;
            healthBarRtoLslider.value = percentageRemaining_thousandths;

            if (percentageRemaining_thousandths >= 250)
            {
                HealthText_GameObject.SetActive(true);
                healthBarcurrentHealth.text = currentHealth.ToString();
                healthBarmaxHealth.text = maxHealth.ToString();
            }
            else
            {
                HealthText_GameObject.SetActive(false);
            }
        }

        public void Disable_TargetInfo()
        {
            TargetInfo_GameObject.SetActive(false);
        }

    }
}
