using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class ToxicBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public Gradient gradient;
    public Animator faceAnimator;
    public float maxDrunkness = 50f;

    private void Start()
    {
        SetMaxDrunkness(maxDrunkness);
    }

    public void SetMaxDrunkness(float toxic)
    {
        slider.maxValue = toxic;
        slider.value = 0;
        fill.color = gradient.Evaluate(0f);
    }

    public void SetDrunkness(float toxic)
    {
        slider.value = toxic;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        faceAnimator.SetFloat("drunkness", toxic);
    }
}
