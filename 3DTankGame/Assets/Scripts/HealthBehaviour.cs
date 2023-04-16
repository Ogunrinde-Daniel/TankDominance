using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBehaviour : MonoBehaviour
{
    public Slider slider;
    public Color lowHealth;
    public Color highHealth;
    public void SetHealth(float health, float maxHealth)
    {
        slider.value = health /maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(lowHealth, highHealth, slider.normalizedValue);
    }


    
}
