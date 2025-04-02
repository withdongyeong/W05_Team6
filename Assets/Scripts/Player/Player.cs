using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    private float _currentEnergy;
    private List<Pilot> _pilots = new();
    private float _currentHp;
    private float _hpMax;
    
    void Start()
    {
        _currentEnergy = GlobalSettings.Instance.PlayerEnergyMax;
        _hpMax = GlobalSettings.Instance.PlayerHpMax;
        _currentHp = _hpMax;
        _pilots.AddRange(GetComponentsInChildren<Pilot>());
        StartCoroutine(EnergyRecoveryRoutine());
    }

    IEnumerator EnergyRecoveryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            _currentEnergy = Mathf.Min(
                GlobalSettings.Instance.PlayerEnergyMax,
                _currentEnergy + GlobalSettings.Instance.PlayerEnergyRecoveryPerSec
            );
        }
    }

    public bool CanIssueCommand(PilotActionData action)
    {
        return _currentEnergy >= action.energyCost;
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
        
        _currentEnergy -= action.energyCost;
        pilot.PrepareAction(action);
    }


    public void CancelCommand(Pilot pilot)
    {
        if (pilot.CancelAction())
        {
            _currentEnergy = Mathf.Min(
                GlobalSettings.Instance.PlayerEnergyMax,
                _currentEnergy + GlobalSettings.Instance.ActionCancelRefundAmount
            );
        }
    }
    
    public bool TakeDamage(float amount)
    {
        _currentHp -= amount;
        Debug.Log($"Player took {amount} damage. Current HP: {_currentHp}");

        if (_currentHp <= 0)
        {
            Debug.Log("PLAYER DEFEATED");
            return false;
        }

        return true;
    }

    public bool IsAlive()
    {
        if (_currentHp <= 0) return false;
        else return true;
    }

}