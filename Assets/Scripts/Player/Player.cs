using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    private PlayerEnergy _playerEnergy;

    private List<Pilot> _pilots = new();
    private float _currentHp;
    private float _hpMax;
    private Tester _tester;

    void Awake()
    {
        _tester = FindAnyObjectByType<Tester>();
        _playerEnergy = GetComponent<PlayerEnergy>();
    }
    private void Start()
    {
        _hpMax = GlobalSettings.Instance.PlayerHpMax;
        _currentHp = _hpMax;
        _pilots.AddRange(GetComponentsInChildren<Pilot>());
    }

    public bool CanIssueCommand(PilotActionData action)
    {
        return _playerEnergy.CurrentEnergy >= action.energyCost;
    }

    public void IssueCommand(Pilot pilot, PilotActionData action)
    {
        if (!pilot.CanAcceptCommand())
        {
            Debug.Log($"Pilot {pilot.pilotId} is currently preparing. Command denied.");
            return;
        }

        if (!CanIssueCommand(action))
        {
            Debug.Log("Not enough energy");
            return;
        }
        _playerEnergy.ChangeEnergy((int)-action.energyCost);
        pilot.PrepareAction(action);
    }


    public void CancelCommand(Pilot pilot)
    {
        if (pilot.CancelAction())
        {
            _playerEnergy.ChangeEnergy((int)GlobalSettings.Instance.ActionCancelRefundAmount);
        }
    }
    
    public bool TakeDamage(float amount)
    {
        _currentHp -= amount;
        if (_tester) _tester.UpdateResultText($"Player took {amount} damage. Current HP: {_currentHp}");

        if (_currentHp <= 0)
        {
            if (_tester) _tester.UpdateResultText("PLAYER DEFEATED");
            return false;
        }

        return true;
    }

    public bool IsAlive()
    {
        if (_currentHp <= 0) return false;
        else return true;
    }

    public void ChangePlayerEnergy(int num)
    {
        _playerEnergy.ChangeEnergy(num);
    }
}