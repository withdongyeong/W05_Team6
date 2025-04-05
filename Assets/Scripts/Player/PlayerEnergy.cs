using System.Collections;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    private float _currentEnergy;
    public float CurrentEnergy { get { return _currentEnergy; } set { _currentEnergy = value; } }

    private void Start()
    {
        _currentEnergy = GlobalSettings.Instance.PlayerEnergyMax;
        StartCoroutine(EnergyRecoveryRoutine());
    }

    IEnumerator EnergyRecoveryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ChangeEnergy((int)GlobalSettings.Instance.PlayerEnergyRecoveryPerSec);
        }
    }

    public void ChangeEnergy(int num)
    {
        CurrentEnergy = Mathf.Clamp(CurrentEnergy + num, 0, GlobalSettings.Instance.PlayerEnergyMax);
        UIManager.Instance.UpdateEnergyUI((int)CurrentEnergy);
    }

}
