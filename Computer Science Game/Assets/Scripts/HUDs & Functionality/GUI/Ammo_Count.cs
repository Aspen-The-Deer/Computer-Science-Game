using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo_Count : MonoBehaviour
{
    Text ammoText;
    public void setAmmo(int ammoCount)
    {
        ammoText = this.GetComponentInChildren<Text>();
        ammoText.text = (ammoCount.ToString() + " / 10");
    }
}
