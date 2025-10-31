using TMPro;
using UnityEngine;
namespace Scott.Barley.v2
{
    /// <summary>
    /// 
    /// 1. Exponential Growth (Simple & Common):    cost = baseCost × (growthRate ^ level)
    /// 2. Quadratic or Polynomial Growth:          cost = baseCost + (level ^ 2) × multiplier
    /// 3. Hybrid (Exponential + Linear Boost):     cost = baseCost × (growthRate ^ level) + level × linearBoost
    /// 
    /// </summary>
    public class UI_Score_Singelton : Singleton<UI_Score_Singelton>
    {
        [Header("Setup")]
        [SerializeField] TMP_Text _score_txt;
        [SerializeField] TMP_Text _scrap_txt;
        [SerializeField] TMP_Text _scrapUpgradeCost_txt;

        [SerializeField] UI_SliderOutputControl _Scrap_SliderOutputControl_L;
        [SerializeField] UI_SliderOutputControl _Scrap_SliderOutputControl_R;

        [Header("Upgrade Cost")]
        [SerializeField] float _baseCost = 100f;
        [SerializeField] float _growthRate = 1.1f;
        [SerializeField] float _linearBoost = 50;
        int _currentLevel = 1;
        int _currentUpgradeCost;

        [Header(" Score")]
        [SerializeField] int _ScoreForLevelUp = 100;


        // External Calls
        UI_UpgradeManager_Singleton _UI_UpgradeManager_Singleton;


        int _score;
        int _scrap;

        private void Start()
        {
            _UI_UpgradeManager_Singleton = UI_UpgradeManager_Singleton.Instance;

            UpdateUpgradeCost();
            _score = 0;
            _score_txt.text = "0";
            _scrap_txt.text = "0";
            _scrapUpgradeCost_txt.text = _currentUpgradeCost.ToString();


            float percentage = (_scrap / (float)_currentUpgradeCost) * 100f;
            percentage = Mathf.Min(percentage, 100f); // Cap at 100%
            _Scrap_SliderOutputControl_L?.fn_SetFillPct_NoLerp(percentage);
            _Scrap_SliderOutputControl_R?.fn_SetFillPct_NoLerp(percentage);
        }
        private void Update()
        {
            Update_UpgradeTrigger();
        }

        public int fn_GetScore() 
        {
            return _score;
        }

        public void fn_AddToScore(int valueToAdd)
        {
            _score += valueToAdd;
            _score_txt.text = _score.ToString();
        }

        public void fn_AddToScrap(int valueToAdd)
        {
            _scrap += valueToAdd;
            _scrap_txt.text = _scrap.ToString();

            float percentage = ((float)_scrap / (float)_currentUpgradeCost);
            percentage = Mathf.Min(percentage, 1f); // Cap at 100% = 1

            _Scrap_SliderOutputControl_L?.fn_SetFillPct_Lerp(percentage);
            _Scrap_SliderOutputControl_R?.fn_SetFillPct_Lerp(percentage);
            Debug.Log($"UI_Score_Singelton; fn_AddToScrap: value: {percentage} pct");
        }

        void UpdateUpgradeCost()
        {
            float cost = _baseCost * Mathf.Pow(_growthRate, _currentLevel) + _currentLevel * _linearBoost;
            _currentUpgradeCost = Mathf.CeilToInt(cost);
            _scrapUpgradeCost_txt.text = _currentUpgradeCost.ToString();
        }

        private void Update_UpgradeTrigger()
        {
            if (_scrap >= _currentUpgradeCost)
            {
                _UI_UpgradeManager_Singleton.fn_CallUpgrade();
                UpdateUpgradeCost();

                int leftOver = _scrap - _currentUpgradeCost;
                _scrap = 0;
       
                _scrap_txt.text = _scrap.ToString();
                _Scrap_SliderOutputControl_L?.fn_SetFillPct_NoLerp(0);
                _Scrap_SliderOutputControl_R?.fn_SetFillPct_NoLerp(0);

                _scrap = leftOver;
                _scrap_txt.text = _scrap.ToString();

                float percentage = ((float)_scrap / (float)_currentUpgradeCost);
                percentage = Mathf.Min(percentage, 1f); // Cap at 100% = 1

                _Scrap_SliderOutputControl_L?.fn_SetFillPct_Lerp(percentage);
                _Scrap_SliderOutputControl_R?.fn_SetFillPct_Lerp(percentage);
            }
        }
    }
}