using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Status : MonoBehaviour, ITab
{
    private Slider energySlider;
    private Text energyText;
    private Slider[] actionSliders;

    private void Awake()
    {
        energyText = transform.GetChild(0).gameObject.GetComponent<Text>();
        energySlider = transform.GetChild(1).GetComponent<Slider>();
    }

    public void StartTab()
    {
        gameObject.SetActive(true);
    }

    public void UpdateTab()
    {
        
    }

    public void StopTab()
    {
        gameObject.SetActive(false);
    }
}
