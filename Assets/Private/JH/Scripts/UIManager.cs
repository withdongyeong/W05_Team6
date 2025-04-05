using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject DefaultScreen;
    public GameObject AttackManualScreen;
    //public GameObject DefenseManualScreen;

    public GameObject EnergyUI;
    public TextMeshProUGUI StatusUI;
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
        _energyUIText = EnergyUI.GetComponentInChildren<TextMeshPro>();
        _energyBar = EnergyUI.GetComponentInChildren<EnergyBar>();

        OnResetUI += SetDefaultScreen;
    }

    public void UpdateEnergyUI(int currentEnergy)
    {
        _energyUIText.text = currentEnergy.ToString() + " / " + GlobalSettings.Instance.PlayerEnergyMax.ToString();
        _energyBar.UpdateEnergyBarUI(currentEnergy);
    }

    public void UpdateStatusUI(string message)
    {
        StatusUI.text = message;
    }

    private void SetDefaultScreen()
    {
        DefaultScreen.SetActive(true);
        AttackManualScreen.SetActive(false);
        //DefenseManualScreen.SetActive(false);
    }

    public void SetAttackManualScreen()
    {
        AttackManualScreen.SetActive(true);
        DefaultScreen.SetActive(false);
        //DefenseManualScreen.SetActive(false);
    }
}
