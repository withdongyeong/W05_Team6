using UnityEngine;
using System.Collections;

public class Pilot : MonoBehaviour
{
    public int pilotId;

    private bool isPreparing = false;
    private bool isCancelled = false;
    private Player _player;
    private Coroutine prepareRoutine;

    public bool CanAcceptCommand() => !isPreparing;

    void Awake()
    {
        _player = GetComponentInParent<Player>();
    }
    
    public void PrepareAction(PilotActionData actionData)
    {
        if (isPreparing) return;

        isPreparing = true;
        isCancelled = false;
        
        prepareRoutine = StartCoroutine(PrepareAndExecute(actionData));
    }

    IEnumerator PrepareAndExecute(PilotActionData actionData)
    {
        Tester tester = FindAnyObjectByType<Tester>();
        if (tester) tester.UpdatePlayerText($"Pilot {pilotId} preparing action {actionData.id}");
        
        yield return new WaitForSeconds(GlobalSettings.Instance.PlayerPrepareTime);

        if (!isCancelled && _player.IsAlive()) // 안정성 확보
        {
            GameManager.Instance.ReceivePlayerAction(pilotId, actionData);
        }

        isPreparing = false;
        prepareRoutine = null;
    }



    public bool CancelAction()
    {
        if (!isPreparing || prepareRoutine == null) return false;

        isCancelled = true;

        StopCoroutine(prepareRoutine);
        prepareRoutine = null;
        isPreparing = false;

        Debug.Log($"Pilot {pilotId} action cancelled.");
        return true;
    }
}