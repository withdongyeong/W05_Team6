using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject _energyUI;
    public TextMeshProUGUI _statusUI;
    public static UIManager Instance;
    public Action OnResetUI;
    public Action OnDisableUI;

    private TextMeshPro _energyUIText;
    private EnergyBar _energyBar;


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

    private void Start()
    {
        _energyUIText = _energyUI.GetComponentInChildren<TextMeshPro>();
        _energyBar = _energyUI.GetComponentInChildren<EnergyBar>();
    }

    public void UpdateEnergyUI(int currentEnergy)
    {
        _energyUIText.text = currentEnergy.ToString() + " / " + GlobalSettings.Instance.PlayerEnergyMax.ToString();
        _energyBar.UpdateEnergyBarUI(currentEnergy);
    }

    public void UpdateStatusUI(string message)
    {
        _statusUI.text = message;
    }
}
