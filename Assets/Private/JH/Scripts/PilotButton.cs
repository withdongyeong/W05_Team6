using Unity.VisualScripting;
using UnityEngine;

public class PilotButton : MonoBehaviour
{
    public string actionId;
    
    private Player _player;
    private Pilot _pilot;
    private PilotActionDataList _pilotActions;


    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _pilot = GetComponentInParent<Pilot>();
        _pilotActions = DataLoader.LoadPilotActions();
    }

    void OnMouseDown()
    {
        Debug.Log("Button Clicked");
        var matchedAction = _pilotActions.pilotActions.Find(a =>
        a.pilot == _pilot.pilotId && a.id == actionId);
        _player.IssueCommand(_pilot, matchedAction);
    }
}
