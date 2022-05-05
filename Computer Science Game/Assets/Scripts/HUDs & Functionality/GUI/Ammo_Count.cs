using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ^ Default unity functionality requirements + UI tools.

public class Ammo_Count : MonoBehaviour
{
    // Creates a reference to
    // the HUD ammo text.
    Text ammoText;

    // A function that is called every frame
    public void setAmmo(int ammoCount)
    {
        // Get the text component of the canvas, and set
        // the ammo count to be the ammo remaining /10.
        ammoText = this.GetComponentInChildren<Text>();
        ammoText.text = (ammoCount.ToString() + " / 10");
    }
}
