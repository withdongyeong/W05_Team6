using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Action OnResetUI;
    public Action OnBackGroundDark;
    public GameObject BackGround;
    public GameObject[] PilotButtons;

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
        Init();
    }

    private void Init()
    {
        SetActiveUI();
        OnResetUI += SetActiveUI;
    }

    private void SetActiveUI()
    {
        for (int i = 0; i < PilotButtons.Length; i++)
        {
            PilotButtons[i].SetActive(true);
        }
    }

    public void ResetUI()
    {
        OnResetUI?.Invoke();
    }

    public void SetBackGround(bool isTrue)
    {
        BackGround.SetActive(isTrue);
    }
}
