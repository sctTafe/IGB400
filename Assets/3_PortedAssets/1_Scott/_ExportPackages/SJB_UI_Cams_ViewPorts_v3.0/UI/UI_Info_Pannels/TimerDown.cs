using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Saw see Dah
/// Description: this script cound down the time by seconds and upon reaching Zero, 
/// </summary>

public class TimerDown : MonoBehaviour
{
    float currentTime;
    public int minuteStart;
    public Text currentTimeText;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = minuteStart * 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0)                                                   
        {
            currentTime -= Time.deltaTime;                                      //Timer counting down -=
            
            if (currentTime <= 0)         
            {
                SceneManager.LoadSceneAsync("LoseScene", LoadSceneMode.Single); //changes the scene to LoseScene upon timer reaching zero
               // currentTime = 0;
                //Debug.Log("Game over");

            }
        }

        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        //TimeSpan time = TimeSpan.FromMilliseconds(currentTime);
        currentTimeText.text = time.ToString("mm\\:ss"); //time.Minutes.ToString() + ":" + time.Seconds.ToString();
        //currentTimeText.text = time.Milliseconds.ToString() + ":" + time.Milliseconds.ToString();
        if (currentTime < 60)
        {
            currentTimeText.color = Color.red;
        }
        else
        {
            currentTimeText.color = Color.white;
        }
        //Debug.Log(currentTime);
    }
}
