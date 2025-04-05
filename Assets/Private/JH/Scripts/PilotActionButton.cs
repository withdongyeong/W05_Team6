using Unity.VisualScripting;
using UnityEngine;

public class PilotActionButton : MonoBehaviour
{
    public string actionId;
    
    private Player _player;
    private int _pilotId;
    private Pilot _pilot;
    private PilotActionDataList _pilotActions;


    private void Start()
    {
        _player = GameManager.Instance.Player;
        _pilotId = GetComponentInParent<PilotButton>().pilotId;
        _pilot = GameManager.Instance.Pilots[_pilotId];
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
