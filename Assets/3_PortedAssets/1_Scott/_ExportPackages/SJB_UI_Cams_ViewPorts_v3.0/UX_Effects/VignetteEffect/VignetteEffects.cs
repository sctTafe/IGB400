using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class VignetteEffects : MonoBehaviour
{

    [SerializeField] private Volume globalVolume;
    private Vignette vignette;

    private int currentUsePriority;


    private void Start()
    {
        currentUsePriority = 100;
        globalVolume.profile.TryGet<Vignette>(out vignette);
     
    }

    private Color color;

    private void FixedUpdate()
    {
        //vignette.color.value = color;
    }
    private int currentActivePriorityLevel;


    private void fnc_custom(int priority = 1, float flashTime = 0.5f)
    { 
    }

    private void Action_ApplyVignetteEffect()
    {

    }



    private void fnc_priority1_redDamageFlash(float flashTime)
    {
        //color = Color.red;
        vignette.color.value = Color.red;
        vignette.intensity.value = 0.4f;
        //Debug.Log("0, :" + flashTime);
        StartCoroutine(ExampleCoroutine(flashTime));

    }



    //public void
    private void example_flashRed(float flashTime)
    {
        //color = Color.red;
        vignette.color.value = Color.red;
        vignette.intensity.value = 0.4f;
        //Debug.Log("0, :" + flashTime);
        StartCoroutine(ExampleCoroutine(flashTime));

    }

    private void action_SetVinetteIntensityToZero()
    {
        vignette.intensity.value = 0.00f;
        vignette.color.value = Color.black;
    }

    IEnumerator ExampleCoroutine(float time)
    {    
        yield return new WaitForSeconds(time);
        action_SetVinetteIntensityToZero();
    }

    private void Update()
    {
        Test_ByKeyDown();
    }

    private void Test_ByKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            example_flashRed(0.25f);
        }
    }


}
