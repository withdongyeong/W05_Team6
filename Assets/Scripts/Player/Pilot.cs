using UnityEngine;
using System.Collections;

public class Pilot : MonoBehaviour
{
    public int pilotId;

    private bool _isPreparing = false;
    private bool _isCancelled = false;
    private Player _player;
    private Coroutine _prepareRoutine;

    public bool CanAcceptCommand() => !_isPreparing;
    public bool IsPreparing => _isPreparing;


    void Awake()
    {
        _player = GetComponentInParent<Player>();
    }
    
    public void PrepareAction(PilotActionData actionData)
    {
        if (_isPreparing) return;

        _isPreparing = true;
        _isCancelled = false;
        
        _prepareRoutine = StartCoroutine(PrepareAndExecute(actionData));
    }

    IEnumerator PrepareAndExecute(PilotActionData actionData)
    {
        Tester tester = FindAnyObjectByType<Tester>();
        if (tester) tester.UpdatePlayerText($"Pilot {pilotId} preparing action {actionData.id}");
        
        yield return new WaitForSeconds(GlobalSettings.Instance.PlayerPrepareTime);

        if (!_isCancelled && _player.IsAlive()) // 안정성 확보
        {
            GameManager.Instance.ReceivePlayerAction(pilotId, actionData);
        }

        _isPreparing = false;
        _prepareRoutine = null;
    }



    public bool CancelAction()
    {
        if (!_isPreparing || _prepareRoutine == null) return false;

        _isCancelled = true;

        StopCoroutine(_prepareRoutine);
        _prepareRoutine = null;
        _isPreparing = false;

        Debug.Log($"Pilot {pilotId} action cancelled.");
        return true;
    }
}