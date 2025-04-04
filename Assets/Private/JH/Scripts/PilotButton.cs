using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PilotButton : MonoBehaviour
{ 
    private Player _player;
    private Pilot _pilot;
    private PilotActionButton[] _pilotActionButtons;
    private Collider2D _collider;


    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _pilot = GetComponentInParent<Pilot>();
        _pilotActionButtons = GetComponentsInChildren<PilotActionButton>(includeInactive:true);
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        UIManager.Instance.OnResetUI += Close;
        for (int i = 0; i < _pilotActionButtons.Length; i++)
        {
            _pilotActionButtons[i].gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        UIManager.Instance.OnResetUI -= Close;
    }

    private void OnMouseDown()
    {
        Open();
    }

    private void Open()
    {
        //UIManager.Instance.OBackGroundDark(
        UIManager.Instance.SetBackGround(true);
        for (int i = 0; i < _pilotActionButtons.Length; i++)
        {
            _pilotActionButtons[i].gameObject.SetActive(true);
        }
        //gameObject.SetActive(false);
    }

    private void Close()
    {
        UIManager.Instance.SetBackGround(false);
        for (int i = 0; i < _pilotActionButtons.Length; i++)
        {
            _pilotActionButtons[i].gameObject.SetActive(false);
        }
    }
}
