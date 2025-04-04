using Unity.VisualScripting;
using UnityEngine;

public class PilotActionButton : MonoBehaviour
{
    public string actionId;
    
    private Player _player;
    private Pilot _pilot;
    private PilotActionDataList _pilotActions;


    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _pilot = GetComponentInParent<Pilot>();
        _pilotActions = DataLoader.LoadPilotActions();
    }


    private void OnMouseDown()
    {
        Debug.Log("Button Clicked");
        var matchedAction = _pilotActions.pilotActions.Find(a =>
        a.pilot == _pilot.pilotId && a.id == actionId);
        _player.IssueCommand(_pilot, matchedAction);

        UIManager.Instance.OnResetUI?.Invoke();
    }
}
