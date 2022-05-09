using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Diagnostics;
// ^ Default unity functionality requirements + UI tools.

public class Health_Bar : MonoBehaviour
{ 
    // Create references tp adjustable UI elements 
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    // On script start
    private void Awake()
    {
        // Set the fill colour of the health bar to match a point
        // in the gradient equivilent to current health.
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void SetHealth(int health)
    {
        // Set the fill colour of the health bar to match a point
        // in the gradient equivilent to current health.
        fill.color = gradient.Evaluate(slider.normalizedValue);
        // Set the value of the slider to the current health.
        slider.value = health;
    }

}
