using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PilotButton : MonoBehaviour
{
    public int pilotId;

    private PilotActionButton[] _pilotActionButtons;
    private Collider2D _collider;


    private void Start()
    {
        _pilotActionButtons = GetComponentsInChildren<PilotActionButton>(includeInactive:true);
        _collider = GetComponent<Collider2D>();

        UIManager.Instance.OnResetUI += Close;
        UIManager.Instance.OnDisableUI += DisableSelf;
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnResetUI -= Close;
        UIManager.Instance.OnDisableUI -= DisableSelf;
    }

    private void OnMouseDown()
    {
        Open();
    }

    private void Open()
    {
        for (int i = 0; i < _pilotActionButtons.Length; i++)
        {
            _pilotActionButtons[i].gameObject.SetActive(true);
        }
        UIManager.Instance.OnDisableUI?.Invoke();
    }

    private void Close()
    {
        _collider.enabled = true;
        for (int i = 0; i < _pilotActionButtons.Length; i++)
        {
            _pilotActionButtons[i].gameObject.SetActive(false);
        }
    }

    private void DisableSelf()
    {
        _collider.enabled = false;
    }
}
