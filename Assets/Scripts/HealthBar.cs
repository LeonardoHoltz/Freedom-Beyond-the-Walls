using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider; // slider to increase/decrease the health bar
    public Gradient gradient; // gradient to change the color of the health bar, based on the remaining health
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        if (health < 0)
            slider.value = 0;
        else
            slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }


}
