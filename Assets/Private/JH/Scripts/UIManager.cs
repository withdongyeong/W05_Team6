using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshPro _energyUIText;
    public static UIManager Instance;
    public Action OnResetUI;
    public Action OnDisableUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateEnergyUI(int currentEnergy)
    {
        _energyUIText.text = currentEnergy.ToString() + " / " + GlobalSettings.Instance.PlayerEnergyMax.ToString();
    }
}
