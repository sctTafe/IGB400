using TMPro;
using UnityEngine;

public class UI_Weapons_Singleton : Singleton<UI_Weapons_Singleton>
{
    public TMP_Text ws1;
    public TMP_Text ws2;
    public TMP_Text ws3;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ws1.text = "0";
        ws2.text = "0";
        ws3.text = "0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void fn_WeaponSlotUpdate(int index, int value)
    {
        if (index == 1)
            fn_WeaponSlot1(value);
        if (index == 2) 
            fn_WeaponSlot2(value);
        if (index == 3)
            fn_WeaponSlot3(value);
    }


    public void fn_WeaponSlot1(int value)
    {
        ws1.text = value.ToString();
    }

    public void fn_WeaponSlot2(int value)
    {
        ws2.text = value.ToString();
    }

    public void fn_WeaponSlot3(int value)
    {
        ws3.text = value.ToString();
    }


}

