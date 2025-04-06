using System.Collections.Generic;
using Unity.VisualScripting;
using SpriteGlow;
using UnityEngine;

public class PilotButton : MonoBehaviour
{
    public int pilotId;
    public SpriteGlowEffect PairingObjectGlow;

    private PilotActionButton[] _pilotActionButtons;
    private SpriteGlowEffect _objectGlow;
    private Collider2D _collider;

    private void Awake()
    {
        _objectGlow = GetComponent<SpriteGlowEffect>();
    }

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
        _objectGlow.OutlineWidth = 10;
        PairingObjectGlow.OutlineWidth = 10;

        for (int i = 0; i < _pilotActionButtons.Length; i++)
        {
            _pilotActionButtons[i].gameObject.SetActive(true);
        }
        UIManager.Instance.OnDisableUI?.Invoke();
    }
     
    private void Close()
    {
        //_objectGlow = GetComponent<SpriteGlowEffect>();
        _objectGlow.OutlineWidth = 0;
        //PairingObjectGlow.OutlineWidth = 0;

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
