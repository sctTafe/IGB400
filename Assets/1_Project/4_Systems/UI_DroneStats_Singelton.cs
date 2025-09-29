using TMPro;

public class UI_DroneStats_Singelton : Singleton<UI_DroneStats_Singelton>
{
    public TMP_Text _signalStrength_txt;
    public TMP_Text _batteryPower_txt;
    
    public TMP_Text _playerLives_txt;

    public TMP_Text _isConnected_txt;
    public TMP_Text _isCharging_txt;

    void Start()
    {
        fn_SetOutput_PlayerLives(3);

        fn_SetOutput_signalStrength(0f);
        fn_SetOutput_batteryPower(0f);

        fn_SetOutput_IsCharging(false);
        fn_SetOutput_IsConnected(false);
    }


    //Int
    public void fn_SetOutput_PlayerLives(int value)
    {
        _playerLives_txt.text = value.ToString();
    }

    // Float
    public void fn_SetOutput_signalStrength(float value)
    {
        _signalStrength_txt.text = value.ToString();
    }
    public void fn_SetOutput_batteryPower(float value)
    {
        _batteryPower_txt.text = value.ToString();
    }


    // Bools
    public void fn_SetOutput_IsCharging(bool isTrue)
    {
        _isCharging_txt.text = isTrue.ToString();
    }

    public void fn_SetOutput_IsConnected(bool isTrue)
    {
        _isConnected_txt.text = isTrue.ToString();
    }


}