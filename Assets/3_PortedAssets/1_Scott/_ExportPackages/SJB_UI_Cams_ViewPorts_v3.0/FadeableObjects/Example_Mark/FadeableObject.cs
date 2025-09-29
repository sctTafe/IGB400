using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Author Mark Hoey
/// Description: This script allows an object to fade in and out based on toggling a boolean
/// </summary>
public class FadeableObject : MonoBehaviour
{
    //Public variables
    public float unfadedAlpha = 1f;
    public float fadedAlpha = 0.2f;
    public float fadeTime = 1;
    public bool isFaded;

    //Private variables
    private bool lastFrameFadedValue;
    private float currentAlpha;
    private float startAlpha;
    private Renderer renderer;
    private float timer;
    private Color originalColor;

    void Start()
    {
        //Cache the renderer component
        renderer = this.GetComponent<Renderer>();
        originalColor = renderer.material.GetColor("_Color");

        //Initially set the object to unfaded alpha value
        currentAlpha = unfadedAlpha;
        renderer.material.SetColor("_Color", new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha));

        //A little error check in case the user puts in zero or a negative number
        if (fadeTime <= 0)
        {
            fadeTime = 1;
        }

        // This line just prevents having to use a divide operation like... timer += Time.deltaTime / fadeTime;
        // Multiplication is a little faster than division but it isn't really necessary to do this.
        fadeTime = 1 / fadeTime;
    }

    void Update()
    {
        //Start the fade timer running (used for alpha Lerp) 
        //if the isFaded boolean is different to the previous frame 
        if (isFaded != lastFrameFadedValue)
        {
            timer = 0;
            startAlpha = currentAlpha;
        }
        //Set what the current isFaded value is for checking on the next frame
        lastFrameFadedValue = isFaded;

        //Fade in or out based on isFaded boolean
        if (isFaded)
        {
            //Only run the timer and the changing of the alpha until the 
            //required value has reached the necessary faded value
            if (currentAlpha > fadedAlpha)
            {
                timer += Time.deltaTime * fadeTime;
                currentAlpha = Mathf.Lerp(startAlpha, fadedAlpha, timer);
                renderer.material.SetColor("_Color", new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha));
            }
        }
        else
        {
            //Only run the timer and the changing of the alpha until the 
            //required value has reached the necessary unfaded value
            if (currentAlpha < unfadedAlpha)
            {
                timer += Time.deltaTime * fadeTime;
                currentAlpha = Mathf.Lerp(startAlpha, unfadedAlpha, timer);
                renderer.material.SetColor("_Color", new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha));
            }
        }
    }
}



