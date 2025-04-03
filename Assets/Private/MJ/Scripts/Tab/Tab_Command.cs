using System;
using UnityEngine;
using UnityEngine.UI;

public class Tab_Command : MonoBehaviour, ITab
{
    private Image energyGauge;
    private Text energyText;

    private void Awake()
    {
        energyGauge = transform.GetChild(1).GetComponent<Image>();
        energyText = transform.GetChild(2).GetComponent<Text>();
        energyGauge.fillAmount = 0;
    }

    public void StartTab()
    {
        gameObject.SetActive(true);
    }

    public void UpdateTab()
    {
        energyGauge.fillAmount += Time.deltaTime * 0.2f;
    }

    public void StopTab()
    {
        gameObject.SetActive(false);
    }
}
