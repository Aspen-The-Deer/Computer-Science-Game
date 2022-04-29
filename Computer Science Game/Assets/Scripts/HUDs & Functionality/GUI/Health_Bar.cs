using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Bar : MonoBehaviour
{ 
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Awake()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void SetHealth(int health)
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
        slider.value = health;
    }

}
