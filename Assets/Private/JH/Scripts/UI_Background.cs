using UnityEngine;

public class UI_Background : MonoBehaviour
{

    private void OnEnable()
    {
        UIManager.Instance.OnResetUI += SetActiveFalse;
    }
    
    private void OnDisable()
    {
        UIManager.Instance.OnResetUI -= SetActiveFalse;
    }
    private void OnMouseDown()
    {
        UIManager.Instance.ResetUI();
    }

    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
